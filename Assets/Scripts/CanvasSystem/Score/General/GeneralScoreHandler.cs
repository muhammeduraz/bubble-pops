using TMPro;
using System;
using UnityEngine;
using Assets.Scripts.Extensions.Numeric;

namespace Assets.Scripts.CanvasSystem.Score.General
{
    public class GeneralScoreHandler : MonoBehaviour, IDisposable
    {
        #region Variables

        private const string MultiplierPrefix = "x";

        private double _score;
        private int _multiplier;

        private float _timer;

        [SerializeField] private float _multiplierExpireDuration;

        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _multiplierText;

        #endregion Variables

        #region Properties

        public int Multiplier { get => _multiplier; set { if (value < 1) value = 1; _multiplier = value; } }

        #endregion Properties

        #region Unity Functions

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            UpdateTimer();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion Unity Functions

        #region Functions

        private void Initialize()
        {
            _score = 0f;
            _multiplier = 1;

            UpdateScoreText();
            UpdateMultiplierText();
        }

        public void Dispose()
        {
            _scoreText = null;
        }

        public double GetScore() => _score;

        private void UpdateTimer()
        {
            if (_multiplier <= 1) return;

            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                ResetTimer();
                ExpireMultiplier();
                UpdateMultiplierText();
            }
        }

        public void UpdateScore(double amount)
        {
            _score += amount * _multiplier;
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            _scoreText.text = _score.AbbrivateNumber();
        }

        public void UpdateMultiplier(int multiplier)
        {
            if (multiplier < _multiplier) return;

            Multiplier = multiplier;
            UpdateMultiplierText();
            ResetTimer();
        }

        private void UpdateMultiplierText()
        {
            _multiplierText.text = MultiplierPrefix + _multiplier;
        }

        private void ExpireMultiplier()
        {
            Multiplier--;
        }

        private void ResetTimer()
        {
            _timer = _multiplierExpireDuration;
        }

        #endregion Functions
    }
}