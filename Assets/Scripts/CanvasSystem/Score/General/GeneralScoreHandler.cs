using TMPro;
using System;
using UnityEngine;
using Assets.Scripts.Extensions.Numeric;

namespace Assets.Scripts.CanvasSystem.Score.General
{
    public class GeneralScoreHandler : MonoBehaviour, IDisposable
    {
        #region Variables

        private double _score;

        [SerializeField] private TextMeshProUGUI _scoreText;

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
            _score = 0f;
            UpdateText();
        }

        public void Dispose()
        {
            _scoreText = null;
        }

        public void UpdateScore(double amount)
        {
            _score += amount * 100;
            UpdateText();
        }

        private void UpdateText()
        {
            _scoreText.text = _score.AbbrivateNumber();
        }

        #endregion Functions
    }
}