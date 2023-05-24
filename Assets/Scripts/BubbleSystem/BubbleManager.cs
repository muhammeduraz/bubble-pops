using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.HapticSystem;
using Assets.Scripts.BubbleSystem.Data;
using Assets.Scripts.BubbleSystem.Creator;
using Assets.Scripts.BubbleSystem.Creator.Data;

namespace Assets.Scripts.BubbleSystem
{
    [DefaultExecutionOrder(-1)]
    public class BubbleManager : MonoBehaviour, IDisposable
    {
        #region Events

        public Action<int> UpdateCombo;
        public Action CameraShakeRequested;
        public Action MergeOperationCompleted;
        public Action<double> UpdateGeneralScore;
        public Action<int, Vector3> ShowBubbleScore;
        public Action<int, Vector3> BubbleParticleRequested;

        #endregion Events
        
        #region Variables

        private int _comboCounter;

        private WaitForSeconds _waitForSeconds_01;
        private WaitForSeconds _waitForSeconds_02;

        private BubbleCreator _bubbleCreator;

        [SerializeField] private int _minActiveBubbleToCreateNewLine;

        [SerializeField] private BubbleDataSO _bubbleDataSO;
        [SerializeField] private BubbleCreatorSettings _bubbleCreatorSettings;

        #endregion Variables

        #region Unity Functions

        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion Unity Functions

        #region Functions

        private void Initialize()
        {
            _waitForSeconds_01 = new WaitForSeconds(0.1f);
            _waitForSeconds_02 = new WaitForSeconds(0.2f);

            _bubbleCreator = new BubbleCreator(_bubbleDataSO, _bubbleCreatorSettings, transform);

            Subscribe(true);
            //StartCoroutine(_bubbleCreator.CreateInitialPile());
            _bubbleCreator.CreateInitialPile();
        }

        public void Dispose()
        {
            Subscribe(false);
        }

        private void Subscribe(bool subscribe)
        {
            if (subscribe)
            {
                _bubbleCreator.BubbleCreated += OnBubbleCreated;
            }
            else
            {
                _bubbleCreator.BubbleCreated -= OnBubbleCreated;
            }
        }

        private void MoveAllBubblesDown()
        {
            Bubble loopBubble = null;

            for (int i = 0; i < _bubbleCreator.ActiveBubbleList.Count; i++)
            {
                loopBubble = _bubbleCreator.ActiveBubbleList[i];

                if (loopBubble != null)
                {
                    loopBubble.MoveDown(_bubbleCreatorSettings.verticalOffset);
                }
            }
        }

        private void MoveDownAndCreateLine()
        {
            if (_bubbleCreator.ActiveBubbleList.Count >= _minActiveBubbleToCreateNewLine) return;

            MoveAllBubblesDown();
            _bubbleCreator.CreateLinePile();
        }
        
        private void HandleCombo()
        {
            _comboCounter++;

            if (_comboCounter > 1)
            {
                UpdateCombo?.Invoke(_comboCounter);
            }
        }

        private void UpdateNeighbourOfEffectedBubbles(List<Bubble> mergedBubbleList)
        {
            Bubble loopBubble = null;
            List<Bubble> effectedBubbleList = GetBubblesThatEffectedByMerge(mergedBubbleList);

            for (int i = 0; i < effectedBubbleList.Count; i++)
            {
                loopBubble = effectedBubbleList[i];
                loopBubble.UpdateNeighbours();
            }
        }

        private List<Bubble> GetBubblesThatEffectedByMerge(List<Bubble> mergedBubbleList)
        {
            List<Bubble> tempBubbleList;
            List<Bubble> fallBubbleList = new List<Bubble>();

            Bubble loopBubble = null;

            for (int i = 0; i < mergedBubbleList.Count; i++)
            {
                tempBubbleList = mergedBubbleList[i].GetNeighbourBubblesWithDifferentId();

                for (int j = 0; j < tempBubbleList.Count; j++)
                {
                    loopBubble = tempBubbleList[j];

                    if (!mergedBubbleList.Contains(loopBubble) && !fallBubbleList.Contains(loopBubble))
                    {
                        fallBubbleList.Add(loopBubble);
                    }
                }
            }

            return fallBubbleList;
        }

        #region Explode Functions

        private void ExplodeBubble(Bubble bubble)
        {
            bubble.ThrowEvent -= MergeProcess;
            bubble.ExplodeEvent -= ExplodeBubble;

            List<Bubble> neighbourList = new List<Bubble>(bubble.NeighbourBubbleList);
            neighbourList.Add(bubble);

            Bubble loopBubble = null;
            for (int i = 0; i < neighbourList.Count; i++)
            {
                loopBubble = neighbourList[i];

                if (loopBubble != null && loopBubble.BubbleData != null)
                {
                    BubbleParticleRequested?.Invoke(loopBubble.BubbleData.id, loopBubble.transform.position);
                    UpdateGeneralScore?.Invoke(loopBubble.BubbleData.id);
                    ShowBubbleScore?.Invoke(loopBubble.BubbleData.id, loopBubble.transform.position);

                    loopBubble.Dispose();
                }
            }

            HandleFall(neighbourList);

            CameraShakeRequested.Invoke();
        }

        #endregion Explode Functions

        #region Fall Functions

        private void HandleFall(List<Bubble> mergedBubbleList)
        {
            List<Bubble> fallBubbleList = GetBubblesThatEffectedByMerge(mergedBubbleList);
            List<Bubble> fallList = new List<Bubble>();

            Bubble loopBubble = null;

            for (int i = 0; i < _bubbleCreator.ActiveBubbleList.Count; i++)
            {
                loopBubble = _bubbleCreator.ActiveBubbleList[i];

                if (loopBubble.IsFallable())
                {
                    fallList.Add(loopBubble);
                }
            }

            fallList.ForEach(dallBubble =>
            {
                _bubbleCreator.ActiveBubbleList.Remove(dallBubble);
                dallBubble.Fall(() => BubbleParticleRequested?.Invoke(dallBubble.BubbleData.id, dallBubble.transform.position));
            });
        }

        #endregion Fall Functions

        #region Merge Functions

        private void MergeProcess(Bubble bubble)
        {
            StartCoroutine(MergeProcessCoroutine(bubble));
        }

        private IEnumerator MergeProcessCoroutine(Bubble bubble)
        {
            _bubbleCreator.AddBubble(bubble);
            bubble.ThrowEvent -= MergeProcess;

            if (!bubble.IsDisposed && bubble.IsMergable())
            {
                yield return StartCoroutine(OnMerge(bubble));
            }
            else
            {
                OnNonMerge();
            }

            MoveDownAndCreateLine();
        }

        private void OnNonMerge()
        {
            _comboCounter = 0;

            MergeOperationCompleted?.Invoke();
        }

        private IEnumerator OnMerge(Bubble bubble)
        {
            List<Bubble> mergedBubbleList = bubble.GetMergeBubbles();
            
            Bubble preferredMergeBubble = GetPreferredMergeBubble(bubble.BubbleData.id, mergedBubbleList);
            mergedBubbleList.Remove(preferredMergeBubble);

            yield return _waitForSeconds_01;

            Bubble loopBubble = null;

            for (int i = 0; i < mergedBubbleList.Count; i++)
            {
                loopBubble = mergedBubbleList[i];
                loopBubble.ExplodeEvent -= ExplodeBubble;

                _bubbleCreator.RemoveBubbleFromCeiling(loopBubble);
                _bubbleCreator.RemoveBubble(loopBubble);

                UpdateGeneralScore?.Invoke(loopBubble.BubbleData.id);
                ShowBubbleScore?.Invoke(loopBubble.BubbleData.id, loopBubble.transform.position);

                BubbleParticleRequested?.Invoke(loopBubble.BubbleData.id, loopBubble.transform.position);

                loopBubble.MoveToDispose(preferredMergeBubble.transform.position);
            }

            HandleCombo();

            yield return _waitForSeconds_02;

            preferredMergeBubble.UpdateBubble(_bubbleDataSO.GetBubbleDataByMultiplication(preferredMergeBubble.BubbleData.id, mergedBubbleList.Count + 1));

            mergedBubbleList.Add(preferredMergeBubble);
            UpdateNeighbourOfEffectedBubbles(mergedBubbleList);

            mergedBubbleList.Remove(preferredMergeBubble);
            HandleFall(mergedBubbleList);

            HapticExtensions.PlayLightHaptic();

            MergeProcess(preferredMergeBubble);
        }

        private Bubble GetPreferredMergeBubble(int id, List<Bubble> mergedBubbleList)
        {
            int newId = _bubbleDataSO.GetMultipliedId(id, mergedBubbleList.Count);

            Bubble loopBubble = null;
            List<Bubble> tempNeighbourList;

            for (int i = 0; i < mergedBubbleList.Count; i++)
            {
                tempNeighbourList = new List<Bubble>(mergedBubbleList[i].NeighbourBubbleList);

                for (int j = 0; j < tempNeighbourList.Count; j++)
                {
                    loopBubble = tempNeighbourList[j];

                    if (loopBubble.BubbleData.id == newId)
                    {
                        return mergedBubbleList[i];
                    }
                }
            }

            return GetHighestPreferredMergeBubble(mergedBubbleList);
        }

        private Bubble GetHighestPreferredMergeBubble(List<Bubble> mergedBubbleList)
        {
            Bubble loopBubble = null;
            Bubble highestBubble = null;
            float highestY = float.NegativeInfinity;

            for (int i = 0; i < mergedBubbleList.Count; i++)
            {
                loopBubble = mergedBubbleList[i];

                if (loopBubble.transform.position.y > highestY)
                {
                    highestBubble = loopBubble;
                    highestY = loopBubble.transform.position.y;
                }
            }

            return highestBubble;
        }

        #endregion Merge Functions

        #region Bubble Data Functions

        public BubbleData OnBubbleDataRequested()
        {
            return _bubbleDataSO.GetRandomBubbleDataByRandomMaxValue();
        }

        #endregion Bubble Data Functions

        #region Creator Functions

        public Bubble OnThrowBubbleRequested()
        {
            Bubble bubble = _bubbleCreator.GetBubble();
            bubble.ThrowEvent += MergeProcess;

            return bubble;
        }

        private void OnBubbleCreated(Bubble bubble)
        {
            bubble.ExplodeEvent += ExplodeBubble;
        }

        #endregion Creator Functions

        #endregion Functions
    }
}