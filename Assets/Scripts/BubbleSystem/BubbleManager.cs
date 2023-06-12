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

        private List<Bubble> _cachedFallList;

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

            _cachedFallList = new List<Bubble>();

            Subscribe(true);
            StartCoroutine(_bubbleCreator.CreateInitialPile());
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

        #region Explode Functions

        private void ExplodeBubble(Bubble bubble)
        {
            bubble.ThrowEvent -= MergeProcess;
            bubble.ExplodeEvent -= ExplodeBubble;

            List<Bubble> neighbourList = bubble.GetNeighbourBubbles();
            HandleFall(neighbourList);
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

                    if (loopBubble.IsCeiling)
                        _bubbleCreator.CeilingBubbleList.Remove(loopBubble);

                    _bubbleCreator.ActiveBubbleList.Remove(loopBubble);
                    loopBubble.Dispose();
                }
            }

            CameraShakeRequested.Invoke();
        }

        #endregion Explode Functions

        #region Fall Functions

        private IEnumerator HandleFall(List<Bubble> matchedBubbleList)
        {
            List<Bubble> fallBubbleList = GetBubblesThatEffectedByMerge(matchedBubbleList);
            _cachedFallList = new List<Bubble>();

            Bubble loopBubble = null;

            for (int i = 0; i < _bubbleCreator.ActiveBubbleList.Count; i++)
            {
                loopBubble = _bubbleCreator.ActiveBubbleList[i];

                if (loopBubble.IsFallable())
                {
                    _cachedFallList.Add(loopBubble);
                }
            }

            if (_cachedFallList.Count < 1) yield return null;

            _cachedFallList.ForEach(fallBubble =>
            {
                _bubbleCreator.ActiveBubbleList.Remove(fallBubble);
                fallBubble.Fall(() => BubbleParticleRequested?.Invoke(fallBubble.BubbleData.id, fallBubble.transform.position));
            });

            HapticExtensions.PlayLightHaptic();
            yield return _waitForSeconds_02;
        }

        private List<Bubble> GetBubblesThatEffectedByMerge(List<Bubble> matchedBubbleList)
        {
            List<Bubble> tempBubbleList;
            List<Bubble> fallBubbleList = new List<Bubble>();

            Bubble loopBubble = null;

            for (int i = 0; i < matchedBubbleList.Count; i++)
            {
                tempBubbleList = matchedBubbleList[i].GetNeighbourBubblesWithDifferentId();

                for (int j = 0; j < tempBubbleList.Count; j++)
                {
                    loopBubble = tempBubbleList[j];

                    if (!matchedBubbleList.Contains(loopBubble) && !fallBubbleList.Contains(loopBubble))
                    {
                        fallBubbleList.Add(loopBubble);
                    }
                }
            }

            return fallBubbleList;
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

                yield return _waitForSeconds_02;

                MoveDownAndCreateLine();
            }
        }

        private void OnNonMerge()
        {
            _comboCounter = 0;

            MergeOperationCompleted?.Invoke();
        }

        private IEnumerator OnMerge(Bubble bubble)
        {
            List<Bubble> mergedBubbleList = bubble.GetMergeBubbles();

            Bubble mergeBubble = GetPreferredMergeBubble(bubble.BubbleData.id, mergedBubbleList);

            if (mergedBubbleList.Contains(mergeBubble))
                mergedBubbleList.Remove(mergeBubble);

            yield return _waitForSeconds_01;

            Bubble loopBubble = null;

            for (int i = 0; i < mergedBubbleList.Count; i++)
            {
                loopBubble = mergedBubbleList[i];
                loopBubble.ExplodeEvent -= ExplodeBubble;

                if (loopBubble.IsCeiling)
                    _bubbleCreator.CeilingBubbleList.Remove(loopBubble);
                _bubbleCreator.ActiveBubbleList.Remove(loopBubble);

                UpdateGeneralScore?.Invoke(loopBubble.BubbleData.id);
                ShowBubbleScore?.Invoke(loopBubble.BubbleData.id, loopBubble.transform.position);

                BubbleParticleRequested?.Invoke(loopBubble.BubbleData.id, loopBubble.transform.position);

                loopBubble.MoveToDispose(mergeBubble.transform.position);
            }

            HandleCombo();
            HapticExtensions.PlayLightHaptic();

            mergeBubble.UpdateBubble(_bubbleDataSO.GetBubbleDataByMultiplication(mergeBubble.BubbleData.id, mergedBubbleList.Count + 1));
            yield return HandleFall(mergedBubbleList);

            MergeProcess(mergeBubble);
        }

        private Bubble GetPreferredMergeBubble(int id, List<Bubble> mergedBubbleList)
        {
            int newId = _bubbleDataSO.GetMultipliedId(id, mergedBubbleList.Count);

            Bubble loopBubble = null;
            List<Bubble> tempNeighbourList;

            for (int i = 0; i < mergedBubbleList.Count; i++)
            {
                tempNeighbourList = mergedBubbleList[i].GetNeighbourBubbles();

                for (int j = 0; j < tempNeighbourList.Count; j++)
                {
                    loopBubble = tempNeighbourList[j];

                    if (!mergedBubbleList.Contains(loopBubble) && loopBubble.BubbleData.id == newId)
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