using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.HapticSystem;
using Assets.Scripts.BubbleSystem.Data;
using Assets.Scripts.BubbleSystem.Creator;

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

        private void HandleFall(List<Bubble> matchedBubbleList)
        {
            List<Bubble> fallBubbleList = GetFallBubbles(matchedBubbleList);
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
                dallBubble.Fall(()=> BubbleParticleRequested?.Invoke(dallBubble.BubbleData.id, dallBubble.transform.position));
            });
        }

        private List<Bubble> GetFallBubbles(List<Bubble> matchedBubbleList)
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

        private Bubble GetMergeBubble(int id, List<Bubble> mergedBubbleList)
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

            return mergedBubbleList[^1];
        }

        private void HandleCombo()
        {
            _comboCounter++;

            if (_comboCounter > 1)
            {
                UpdateCombo?.Invoke(_comboCounter);
            }
        }

        private void ExplodeBubble(Bubble bubble)
        {
            bubble.ThrowEvent -= MatchProcess;
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

        private void MatchProcess(Bubble bubble)
        {
            StartCoroutine(MatchProcessCoroutine(bubble));
        }

        private IEnumerator MatchProcessCoroutine(Bubble bubble)
        {
            _bubbleCreator.AddBubble(bubble);
            bubble.ThrowEvent -= MatchProcess;

            List<Bubble> neighbourBubbleList = bubble.GetNeighbourBubbles();

            if (IsThereAnyMatch(bubble, neighbourBubbleList) && !bubble.IsDisposed)
            {
                yield return StartCoroutine(OnMatch(bubble));
            }
            else
            {
                OnNonMatch();
            }

            MoveDownAndCreateLine();
        }

        private IEnumerator OnMatch(Bubble bubble)
        {
            List<Bubble> mergedBubbleList = GetBubblesWithSameId(bubble);

            Bubble mergeBubble = GetMergeBubble(bubble.BubbleData.id, mergedBubbleList);

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

            yield return _waitForSeconds_02;

            mergeBubble.UpdateBubble(_bubbleDataSO.GetBubbleDataByMultiplication(mergeBubble.BubbleData.id, mergedBubbleList.Count + 1));
            HandleFall(mergedBubbleList);
            HapticExtensions.PlayLightHaptic();

            MatchProcess(mergeBubble);
        }

        private void OnNonMatch()
        {
            _comboCounter = 0;
            
            MergeOperationCompleted?.Invoke();
        }

        private bool IsThereAnyMatch(Bubble bubble, List<Bubble> neighbourBubbleList)
        {
            Bubble loopBubble = null;

            for (int i = 0; i < neighbourBubbleList.Count; i++)
            {
                loopBubble = neighbourBubbleList[i];

                if (loopBubble.BubbleData.id == bubble.BubbleData.id)
                {
                    return true;
                }
            }

            return false;
        }

        private List<Bubble> GetBubblesWithSameId(Bubble bubble)
        {
            Bubble loopBubble = null;

            List<Bubble> tempBubbles;
            List<Bubble> finalBubbles = new List<Bubble>();
            finalBubbles.Add(bubble);

            tempBubbles = bubble.GetNeighbourBubblesWithSameId();

            for (int i = 0; i < tempBubbles.Count; i++)
            {
                loopBubble = tempBubbles[i];

                if (!finalBubbles.Contains(loopBubble))
                {
                    finalBubbles.Add(loopBubble);
                    tempBubbles.AddRange(loopBubble.GetNeighbourBubblesWithSameId());
                }
            }

            return finalBubbles;
        }

        #endregion Functions

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
            bubble.ThrowEvent += MatchProcess;

            return bubble;
        }

        private void OnBubbleCreated(Bubble bubble)
        {
            bubble.ExplodeEvent += ExplodeBubble;
        }

        #endregion Creator Functions
    }
}