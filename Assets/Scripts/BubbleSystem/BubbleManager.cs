using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Particle;
using System.Collections.Generic;
using Assets.Scripts.HapticSystem;
using Assets.Scripts.BubbleSystem.Pool;
using Assets.Scripts.BubbleSystem.Data;
using Assets.Scripts.BubbleSystem.Factory;
using Assets.Scripts.CanvasSystem.Score.Combo;
using Assets.Scripts.CanvasSystem.Score.General;
using Assets.Scripts.CanvasSystem.Score.BubbleScore;

namespace Assets.Scripts.BubbleSystem
{
    [DefaultExecutionOrder(-1)]
    public class BubbleManager : MonoBehaviour, IDisposable
    {
        #region Variables

        private int _verticalOffsetIndex;

        private List<Bubble> _activeBubbleList;

        private BubblePool _bubblePool;
        private BubbleFactory _bubbleFactory;
        private BubbleThrower _bubbleThrower;

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

            _particlePlayer = FindObjectOfType<ParticlePlayer>();
            _bubbleThrower = FindObjectOfType<BubbleThrower>();
            _bubbleFactory = new BubbleFactory(_bubblePrefab);
            _bubblePool = new BubblePool(_bubbleFactory);

            _scoreHandler = FindObjectOfType<BubbleScoreHandler>();
            _comboHandler = FindObjectOfType<BubbleComboHandler>();
            _generalScoreHandler = FindObjectOfType<GeneralScoreHandler>();
            
            _activeBubbleList = new List<Bubble>();

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

            for (int i = 0; i < _initialLineCount; i++)
            {
                StartCoroutine(CreateLinePile(spawnPosition));
                yield return new WaitForSeconds(0.1f);
                spawnPosition.y += _verticalOffset;
            }
        }

        private IEnumerator CreateLinePile(Vector3 spawnPosition)
        {
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

                spawnPosition.x += _horizontalOffset;

                yield return new WaitForSeconds(0.05f);
            }
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

            _activeBubbleList.Remove(bubble);

            List<Bubble> neighbourList = bubble.GetNeighbourBubbles();
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
                }

                loopBubble.Dispose();
            }
        }

        private void MatchProcess(Bubble bubble)
        {
            AddBubble(bubble);
            bubble.ThrowEvent -= MatchProcess;

            List<Bubble> neighbourBubbleList = bubble.GetNeighbourBubbles();

            if (IsThereAnyMatch(bubble, neighbourBubbleList))
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
            List<Bubble> matchedBubbles = GetBubblesWithSameId(bubble);

            yield return new WaitForSeconds(0.1f);

            HandleCombo();

            Bubble loopBubble = null;

            for (int i = 0; i < matchedBubbles.Count - 1; i++)
            {
                loopBubble = matchedBubbles[i];
                _activeBubbleList.Remove(loopBubble);
                loopBubble.ExplodeEvent -= ExplodeBubble;

                _generalScoreHandler.UpdateScore(loopBubble.BubbleData.id);
                _scoreHandler.ShowScore(loopBubble.BubbleData.id, loopBubble.transform.position);
                
                _particlePlayer.PlayParticle(loopBubble.BubbleData.id, loopBubble.transform.position);

                loopBubble.MoveToDispose(matchedBubbles[^1].transform.position);
            }

            yield return new WaitForSeconds(0.2f);

            matchedBubbles[^1].UpdateBubble(_bubbleDataSO.GetBubbleDataByMultiplication(matchedBubbles[^1].BubbleData.id, matchedBubbles.Count));
            HapticExtensions.PlayLightHaptic();

            MatchProcess(matchedBubbles[^1]);
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

            tempBubbles = bubble.GetNeighbourBubblesWithTheSameId();

            for (int i = 0; i < tempBubbles.Count; i++)
            {
                loopBubble = tempBubbles[i];

                if (!finalBubbles.Contains(loopBubble))
                {
                    finalBubbles.Add(loopBubble);
                    tempBubbles.AddRange(loopBubble.GetNeighbourBubblesWithTheSameId());
                }
            }

            return finalBubbles;
        }

        #endregion Functions
    }
}