using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Particle;
using System.Collections.Generic;
using Assets.Scripts.HapticSystem;
using Assets.Scripts.CameraSystem;
using Assets.Scripts.BubbleSystem.Pool;
using Assets.Scripts.BubbleSystem.Data;
using Assets.Scripts.BubbleSystem.Factory;
using Assets.Scripts.CanvasSystem.Score.Combo;
using Assets.Scripts.CanvasSystem.Score.General;
using Assets.Scripts.CanvasSystem.Score.BubbleScore;
using DG.Tweening;

namespace Assets.Scripts.BubbleSystem
{
    [DefaultExecutionOrder(-1)]
    public class BubbleManager : MonoBehaviour, IDisposable
    {
        #region Variables

        private int _verticalOffsetIndex;

        private List<Bubble> _activeBubbleList;
        private List<Bubble> _ceilingBubbleList;

        private BubblePool _bubblePool;
        private BubbleFactory _bubbleFactory;
        private BubbleThrower _bubbleThrower;

        private CameraService _cameraService;
        private ParticlePlayer _particlePlayer;
        private BubbleScoreHandler _scoreHandler;
        private BubbleComboHandler _comboHandler;
        private GeneralScoreHandler _generalScoreHandler;

        private int _comboCounter;

        [SerializeField] private float _verticalOffset;
        [SerializeField] private float _horizontalOffset;
        [SerializeField] private Vector3 _initialSpawnPosition;

        [SerializeField] private int _lineSize;
        [SerializeField] private int _initialLineCount;

        [SerializeField] private int _randomMaxExclusive;
        [SerializeField] private int _minActiveBubbleToCreateNewLine;

        [SerializeField] private Bubble _bubblePrefab;
        [SerializeField] private BubbleDataSO _bubbleDataSO;

        #endregion Variables

        #region Properties

        private int VerticalOffsetIndex 
        { 
            get => _verticalOffsetIndex;
            set 
            {
                if (value == 2) value = 0;
                _verticalOffsetIndex = value;
            } 
        }

        public BubbleData GetRandomBubbleData { get => _bubbleDataSO.GetRandomBubbleData(_randomMaxExclusive); }

        #endregion Properties

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
            VerticalOffsetIndex = 0;

            _cameraService = FindObjectOfType<CameraService>();

            _particlePlayer = FindObjectOfType<ParticlePlayer>();
            _bubbleThrower = FindObjectOfType<BubbleThrower>();
            _bubbleFactory = new BubbleFactory(_bubblePrefab);
            _bubblePool = new BubblePool(_bubbleFactory);

            _scoreHandler = FindObjectOfType<BubbleScoreHandler>();
            _comboHandler = FindObjectOfType<BubbleComboHandler>();
            _generalScoreHandler = FindObjectOfType<GeneralScoreHandler>();
            
            _activeBubbleList = new List<Bubble>();
            _ceilingBubbleList = new List<Bubble>();

            StartCoroutine(CreateInitialPile());
        }

        public void Dispose()
        {
            _bubblePool = null;
            _bubbleFactory = null;
        }

        private void AddBubble(Bubble bubble)
        {
            if (_activeBubbleList.Contains(bubble)) return;

            _activeBubbleList.Add(bubble);
        }

        private void RemoveBubble(Bubble bubble)
        {
            _activeBubbleList.Remove(bubble);
        }

        public Bubble GetBubble(bool withSubscription = true)
        {
            Bubble bubble = _bubblePool.GetProduct();

            bubble.transform.position = _initialSpawnPosition;
            bubble.transform.SetParent(transform, true);
            bubble.ExplodeEvent += ExplodeBubble;

            if (withSubscription)
                bubble.ThrowEvent += MatchProcess;

            return bubble;
        }

        private IEnumerator CreateInitialPile()
        {
            Vector3 spawnPosition = _initialSpawnPosition;
            spawnPosition.y += (_verticalOffset * (_initialLineCount - 1));

            for (int i = 0; i < _initialLineCount; i++)
            {
                StartCoroutine(CreateLinePile(spawnPosition));
                yield return new WaitForSeconds(0.1f);
                spawnPosition.y -= _verticalOffset;
            }
        }

        private IEnumerator CreateLinePile(Vector3 spawnPosition)
        {
            ResetCeilingBubbles();

            Bubble instantiatedBubble = null;
            spawnPosition.x -= 0.5f * (VerticalOffsetIndex % 2);
            VerticalOffsetIndex++;

            for (int j = 0; j < _lineSize; j++)
            {
                instantiatedBubble = GetBubble(false);
                instantiatedBubble.transform.position = spawnPosition + Vector3.down * _verticalOffset;

                instantiatedBubble.MoveTo(spawnPosition);
                instantiatedBubble.ScaleOut();

                instantiatedBubble.Initialize();
                instantiatedBubble.UpdateBubble(_bubbleDataSO.GetRandomBubbleData(_randomMaxExclusive));
                instantiatedBubble.SendToPool += RemoveBubble;
                AddBubble(instantiatedBubble);
                
                AddBubbleToCeiling(instantiatedBubble);

                spawnPosition.x += _horizontalOffset;

                yield return new WaitForSeconds(0.05f);
            }
        }

        private void AddBubbleToCeiling(Bubble bubble)
        {
            bubble.IsCeiling = true;
            _ceilingBubbleList.Add(bubble);
        }

        private void ResetCeilingBubbles()
        {
            Bubble loopBubble = null;

            for (int i = 0; i < _ceilingBubbleList.Count; i++)
            {
                loopBubble = _ceilingBubbleList[i];
                loopBubble.IsCeiling = false;
            }

            _ceilingBubbleList.Clear();
        }

        private void MoveAllBubblesDown()
        {
            Bubble loopBubble = null;

            for (int i = 0; i < _activeBubbleList.Count; i++)
            {
                loopBubble = _activeBubbleList[i];

                if (loopBubble != null)
                {
                    loopBubble.MoveDown(_verticalOffset);
                }
            }
        }

        private void MoveDownAndCreateLine()
        {
            if (_activeBubbleList.Count >= _minActiveBubbleToCreateNewLine) return;

            MoveAllBubblesDown();
            StartCoroutine(CreateLinePile(_initialSpawnPosition));
        }

        private void HandleFall(List<Bubble> matchedBubbleList)
        {
            List<Bubble> fallBubbleList = GetFallBubbles(matchedBubbleList);
            List<Bubble> fallList = new List<Bubble>();

            Bubble loopBubble = null;

            for (int i = 0; i < _activeBubbleList.Count; i++)
            {
                loopBubble = _activeBubbleList[i];

                if (loopBubble.IsFallable())
                {
                    fallList.Add(loopBubble);
                }
            }

            fallList.ForEach(x =>
            {
                _activeBubbleList.Remove(x);
                x.Fall(()=> _particlePlayer.PlayParticle(x.BubbleData.id, x.transform.position));
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
                _comboHandler.ShowCombo(_comboCounter);
                _generalScoreHandler.UpdateMultiplier(_comboCounter);
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
                    _particlePlayer.PlayParticle(loopBubble.BubbleData.id, loopBubble.transform.position);
                    _generalScoreHandler.UpdateScore(loopBubble.BubbleData.id);
                    _scoreHandler.ShowScore(loopBubble.BubbleData.id, loopBubble.transform.position);

                    if (loopBubble.IsCeiling)
                        _ceilingBubbleList.Remove(loopBubble);

                    _activeBubbleList.Remove(loopBubble);
                    loopBubble.Dispose();
                }
            }

            _cameraService.ShakeCamera();
        }

        private void MatchProcess(Bubble bubble)
        {
            AddBubble(bubble);
            bubble.ThrowEvent -= MatchProcess;

            List<Bubble> neighbourBubbleList = bubble.GetNeighbourBubbles();

            if (IsThereAnyMatch(bubble, neighbourBubbleList) && !bubble.IsDisposed)
            {
                StartCoroutine(OnMatch(bubble));
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

            yield return new WaitForSeconds(0.1f);

            Bubble loopBubble = null;

            for (int i = 0; i < mergedBubbleList.Count; i++)
            {
                loopBubble = mergedBubbleList[i];
                loopBubble.ExplodeEvent -= ExplodeBubble;

                if (loopBubble.IsCeiling)
                    _ceilingBubbleList.Remove(loopBubble);
                _activeBubbleList.Remove(loopBubble);

                _generalScoreHandler.UpdateScore(loopBubble.BubbleData.id);
                _scoreHandler.ShowScore(loopBubble.BubbleData.id, loopBubble.transform.position);
                
                _particlePlayer.PlayParticle(loopBubble.BubbleData.id, loopBubble.transform.position);

                loopBubble.MoveToDispose(mergeBubble.transform.position);
            }

            HandleCombo();

            yield return new WaitForSeconds(0.2f);

            mergeBubble.UpdateBubble(_bubbleDataSO.GetBubbleDataByMultiplication(mergeBubble.BubbleData.id, mergedBubbleList.Count + 1));
            HandleFall(mergedBubbleList);
            HapticExtensions.PlayLightHaptic();

            MatchProcess(mergeBubble);
        }

        private void OnNonMatch()
        {
            _comboCounter = 0;
            _bubbleThrower.IsThrowActive = true;
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
    }
}