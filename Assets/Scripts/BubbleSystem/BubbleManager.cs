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
        public Action MoveAllBubblesDownCompleted;
        public Action<int, Vector3> ShowBubbleScore;
        public Action<int, Vector3> BubbleParticleRequested;

        #endregion Events
        
        #region Variables

        private int _comboCounter;

        private WaitForSeconds _waitForSeconds_01;
        private WaitForSeconds _waitForSeconds_02;

        private BubbleCreator _bubbleCreator;

        private Bubble _cachedBubble;
        private List<Bubble> _cachedFallList;
        private List<Bubble> _cachedNeighbourList;

        [SerializeField] private BubbleDataSO _bubbleDataSO;
        [SerializeField] private BubbleManagerSettings _bubbleManagerSettings;
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
            _cachedNeighbourList = new List<Bubble>();

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

        public void OnFailed()
        {
            FallAllBubbles();
        }

        private IEnumerator MoveAllBubblesDown()
        {
            for (int i = 0; i < _bubbleCreator.ActiveBubbleList.Count; i++)
            {
                _cachedBubble = _bubbleCreator.ActiveBubbleList[i];

                if (_cachedBubble != null)
                {
                    _cachedBubble.MoveDown(_bubbleCreatorSettings.verticalOffset);
                }
            }

            _cachedBubble = null;

            yield return _waitForSeconds_02;

            MoveAllBubblesDownCompleted?.Invoke();
        }

        private void MoveDownAndCreateLine()
        {
            if (_bubbleCreator.ActiveBubbleList.Count >= _bubbleManagerSettings.minActiveBubbleToCreateNewLine) return;

            StartCoroutine(MoveAllBubblesDown());
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
            bubble.ThrowEvent -= OnThrow;
            bubble.ExplodeEvent -= ExplodeBubble;

            _cachedNeighbourList = bubble.GetNeighbourBubbles();
            HandleFall(_cachedNeighbourList);
            _cachedNeighbourList.Add(bubble);

            for (int i = 0; i < _cachedNeighbourList.Count; i++)
            {
                _cachedBubble = _cachedNeighbourList[i];

                if (_cachedBubble != null && _cachedBubble.BubbleData != null)
                {
                    BubbleParticleRequested?.Invoke(_cachedBubble.BubbleData.id, _cachedBubble.transform.position);
                    UpdateGeneralScore?.Invoke(_cachedBubble.BubbleData.id);
                    ShowBubbleScore?.Invoke(_cachedBubble.BubbleData.id, _cachedBubble.transform.position);

                    if (_cachedBubble.IsCeiling)
                        _bubbleCreator.CeilingBubbleList.Remove(_cachedBubble);

                    _bubbleCreator.ActiveBubbleList.Remove(_cachedBubble);
                    _cachedBubble.Dispose();
                }
            }

            _cachedBubble = null;
            CameraShakeRequested.Invoke();
        }

        #endregion Explode Functions

        #region Fall Functions

        private void HandleFall(List<Bubble> matchedBubbleList)
        {
            List<Bubble> fallBubbleList = GetBubblesThatEffectedByMerge(matchedBubbleList);
            _cachedFallList = new List<Bubble>();
            GetFallableBubblesFromActiveList(_cachedFallList);

            _cachedFallList.ForEach(fallBubble =>
            {
                _bubbleCreator.ActiveBubbleList.Remove(fallBubble);
                fallBubble.Fall(() => BubbleParticleRequested?.Invoke(fallBubble.BubbleData.id, fallBubble.transform.position));
            });

            _cachedBubble = null;
            HapticExtensions.PlayLightHaptic();
        }

        private void GetFallableBubblesFromActiveList(List<Bubble> result)
        {
            for (int i = 0; i < _bubbleCreator.ActiveBubbleList.Count; i++)
            {
                _cachedBubble = _bubbleCreator.ActiveBubbleList[i];

                if (_cachedBubble.IsFallable())
                {
                    result.Add(_cachedBubble);
                }
            }
        }

        private List<Bubble> GetBubblesThatEffectedByMerge(List<Bubble> matchedBubbleList)
        {
            List<Bubble> tempBubbleList;
            List<Bubble> fallBubbleList = new List<Bubble>();

            for (int i = 0; i < matchedBubbleList.Count; i++)
            {
                tempBubbleList = matchedBubbleList[i].GetNeighbourBubblesWithDifferentId();

                for (int j = 0; j < tempBubbleList.Count; j++)
                {
                    _cachedBubble = tempBubbleList[j];

                    if (!matchedBubbleList.Contains(_cachedBubble) && !fallBubbleList.Contains(_cachedBubble))
                    {
                        fallBubbleList.Add(_cachedBubble);
                    }
                }
            }

            _cachedBubble = null;

            return fallBubbleList;
        }

        private void FallAllBubbles()
        {
            _bubbleCreator.ActiveBubbleList.ForEach(fallBubble =>
            {
                fallBubble.Fall(() => BubbleParticleRequested?.Invoke(fallBubble.BubbleData.id, fallBubble.transform.position));
            });
        }

        #endregion Fall Functions

        #region Merge Functions

        private void OnThrow(Bubble bubble)
        {
            bubble.IsThrowBubble = false;
            bubble.ThrowEvent -= OnThrow;

            _bubbleCreator.AddBubble(bubble);

            MergeProcess(bubble);
        }

        private void MergeProcess(Bubble bubble)
        {
            StartCoroutine(MergeProcessCoroutine(bubble));
        }

        private IEnumerator MergeProcessCoroutine(Bubble bubble)
        {
            if (!bubble.IsDisposed && !bubble.IsExploded && bubble.IsMergable())
            {
                yield return StartCoroutine(OnMerge(bubble));
            }
            else
            {
                MoveDownAndCreateLine();

                yield return _waitForSeconds_02;

                OnNonMerge();
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

            for (int i = 0; i < mergedBubbleList.Count; i++)
            {
                _cachedBubble = mergedBubbleList[i];
                _cachedBubble.ExplodeEvent -= ExplodeBubble;

                if (_cachedBubble.IsCeiling)
                    _bubbleCreator.CeilingBubbleList.Remove(_cachedBubble);
                _bubbleCreator.ActiveBubbleList.Remove(_cachedBubble);

                UpdateGeneralScore?.Invoke(_cachedBubble.BubbleData.id);
                ShowBubbleScore?.Invoke(_cachedBubble.BubbleData.id, _cachedBubble.transform.position);

                BubbleParticleRequested?.Invoke(_cachedBubble.BubbleData.id, _cachedBubble.transform.position);

                _cachedBubble.MoveToDispose(mergeBubble.transform.position);
            }

            _cachedBubble = null;

            HandleCombo();
            HapticExtensions.PlayLightHaptic();

            yield return _waitForSeconds_02;
            
            mergeBubble.UpdateBubble(_bubbleDataSO.GetBubbleDataByMultiplication(mergeBubble.BubbleData.id, mergedBubbleList.Count + 1));
            
            HandleFall(mergedBubbleList);

            yield return _waitForSeconds_01;

            MergeProcess(mergeBubble);
        }

        private Bubble GetPreferredMergeBubble(int id, List<Bubble> mergedBubbleList)
        {
            int newId = _bubbleDataSO.GetMultipliedId(id, mergedBubbleList.Count);

            List<Bubble> tempNeighbourList;

            for (int i = 0; i < mergedBubbleList.Count; i++)
            {
                tempNeighbourList = mergedBubbleList[i].GetNeighbourBubbles();

                for (int j = 0; j < tempNeighbourList.Count; j++)
                {
                    _cachedBubble = tempNeighbourList[j];

                    if (!mergedBubbleList.Contains(_cachedBubble) && _cachedBubble.BubbleData.id == newId)
                    {
                        return mergedBubbleList[i];
                    }
                }
            }

            _cachedBubble = GetPreferredMergeBubbleThatHasNeighboursWithDifferentId(mergedBubbleList);

            if (_cachedBubble != null)
                return _cachedBubble;

            _cachedBubble = GetHighestPreferredMergeBubble(mergedBubbleList);
            return _cachedBubble;
        }

        private Bubble GetPreferredMergeBubbleThatHasNeighboursWithDifferentId(List<Bubble> mergedBubbleList)
        {
            for (int i = 0; i < mergedBubbleList.Count; i++)
            {
                _cachedBubble = mergedBubbleList[i];

                if (_cachedBubble.GetNeighbourBubblesWithDifferentId().Count > 0)
                {
                    return _cachedBubble;
                }
            }

            return null;
        }

        private Bubble GetHighestPreferredMergeBubble(List<Bubble> mergedBubbleList)
        {
            Bubble highestBubble = null;
            float highestY = float.NegativeInfinity;

            for (int i = 0; i < mergedBubbleList.Count; i++)
            {
                _cachedBubble = mergedBubbleList[i];

                if (_cachedBubble.transform.position.y > highestY)
                {
                    highestBubble = _cachedBubble;
                    highestY = _cachedBubble.transform.position.y;
                }
            }

            _cachedBubble = null;

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
            _cachedBubble = _bubbleCreator.GetBubble();
            _cachedBubble.ThrowEvent += OnThrow;
            _cachedBubble.IsThrowBubble = true;

            return _cachedBubble;
        }

        private void OnBubbleCreated(Bubble bubble)
        {
            bubble.ExplodeEvent += ExplodeBubble;
        }

        #endregion Creator Functions

        #endregion Functions
    }
}