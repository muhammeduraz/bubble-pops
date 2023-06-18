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

        private Bubble _cachedTempBubble;
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

        private IEnumerator MoveAllBubblesDown()
        {
            for (int i = 0; i < _bubbleCreator.ActiveBubbleList.Count; i++)
            {
                _cachedTempBubble = _bubbleCreator.ActiveBubbleList[i];

                if (_cachedTempBubble != null)
                {
                    _cachedTempBubble.MoveDown(_bubbleCreatorSettings.verticalOffset);
                }
            }

            _cachedTempBubble = null;

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
                _cachedTempBubble = _cachedNeighbourList[i];

                if (_cachedTempBubble != null && _cachedTempBubble.BubbleData != null)
                {
                    BubbleParticleRequested?.Invoke(_cachedTempBubble.BubbleData.id, _cachedTempBubble.transform.position);
                    UpdateGeneralScore?.Invoke(_cachedTempBubble.BubbleData.id);
                    ShowBubbleScore?.Invoke(_cachedTempBubble.BubbleData.id, _cachedTempBubble.transform.position);

                    if (_cachedTempBubble.IsCeiling)
                        _bubbleCreator.CeilingBubbleList.Remove(_cachedTempBubble);

                    _bubbleCreator.ActiveBubbleList.Remove(_cachedTempBubble);
                    _cachedTempBubble.Dispose();
                }
            }

            _cachedTempBubble = null;
            CameraShakeRequested.Invoke();
        }

        #endregion Explode Functions

        #region Fall Functions

        private void HandleFall(List<Bubble> matchedBubbleList)
        {
            List<Bubble> fallBubbleList = GetBubblesThatEffectedByMerge(matchedBubbleList);
            _cachedFallList = new List<Bubble>();

            for (int i = 0; i < _bubbleCreator.ActiveBubbleList.Count; i++)
            {
                _cachedTempBubble = _bubbleCreator.ActiveBubbleList[i];

                if (_cachedTempBubble.IsFallable())
                {
                    _cachedFallList.Add(_cachedTempBubble);
                }
            }

            _cachedFallList.ForEach(fallBubble =>
            {
                _bubbleCreator.ActiveBubbleList.Remove(fallBubble);
                fallBubble.Fall(() => BubbleParticleRequested?.Invoke(fallBubble.BubbleData.id, fallBubble.transform.position));
            });

            _cachedTempBubble = null;
            HapticExtensions.PlayLightHaptic();
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
                    _cachedTempBubble = tempBubbleList[j];

                    if (!matchedBubbleList.Contains(_cachedTempBubble) && !fallBubbleList.Contains(_cachedTempBubble))
                    {
                        fallBubbleList.Add(_cachedTempBubble);
                    }
                }
            }

            _cachedTempBubble = null;

            return fallBubbleList;
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
                _cachedTempBubble = mergedBubbleList[i];
                _cachedTempBubble.ExplodeEvent -= ExplodeBubble;

                if (_cachedTempBubble.IsCeiling)
                    _bubbleCreator.CeilingBubbleList.Remove(_cachedTempBubble);
                _bubbleCreator.ActiveBubbleList.Remove(_cachedTempBubble);

                UpdateGeneralScore?.Invoke(_cachedTempBubble.BubbleData.id);
                ShowBubbleScore?.Invoke(_cachedTempBubble.BubbleData.id, _cachedTempBubble.transform.position);

                BubbleParticleRequested?.Invoke(_cachedTempBubble.BubbleData.id, _cachedTempBubble.transform.position);

                _cachedTempBubble.MoveToDispose(mergeBubble.transform.position);
            }

            _cachedTempBubble = null;

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
                    _cachedTempBubble = tempNeighbourList[j];

                    if (!mergedBubbleList.Contains(_cachedTempBubble) && _cachedTempBubble.BubbleData.id == newId)
                    {
                        return mergedBubbleList[i];
                    }
                }
            }

            _cachedTempBubble = GetPreferredMergeBubbleThatHasNeighboursWithDifferentId(mergedBubbleList);

            if (_cachedTempBubble != null)
                return _cachedTempBubble;

            _cachedTempBubble = GetHighestPreferredMergeBubble(mergedBubbleList);
            return _cachedTempBubble;
        }

        private Bubble GetPreferredMergeBubbleThatHasNeighboursWithDifferentId(List<Bubble> mergedBubbleList)
        {
            for (int i = 0; i < mergedBubbleList.Count; i++)
            {
                _cachedTempBubble = mergedBubbleList[i];

                if (_cachedTempBubble.GetNeighbourBubblesWithDifferentId().Count > 0)
                {
                    return _cachedTempBubble;
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
                _cachedTempBubble = mergedBubbleList[i];

                if (_cachedTempBubble.transform.position.y > highestY)
                {
                    highestBubble = _cachedTempBubble;
                    highestY = _cachedTempBubble.transform.position.y;
                }
            }

            _cachedTempBubble = null;

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
            _cachedTempBubble = _bubbleCreator.GetBubble();
            _cachedTempBubble.ThrowEvent += OnThrow;
            _cachedTempBubble.IsThrowBubble = true;

            return _cachedTempBubble;
        }

        private void OnBubbleCreated(Bubble bubble)
        {
            bubble.ExplodeEvent += ExplodeBubble;
        }

        #endregion Creator Functions

        #endregion Functions
    }
}