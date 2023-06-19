using TMPro;
using System;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.Extensions.Numeric;
using Assets.Scripts.CanvasSystem.Buttons;
using Assets.Scripts.CanvasSystem.Menus.Data;

namespace Assets.Scripts.CanvasSystem.Menus
{
    public class GameFailMenu : MonoBehaviour, IDisposable
    {
        #region Events

        public delegate double GeneralScoreRequested();
        public GeneralScoreRequested RequestGeneralScore;

        #endregion Events

        #region Variables

        private readonly string Score = "Score: ";

        private Sequence _sequence;

        [SerializeField] private RetryButton _retryButton;
        [SerializeField] private TextMeshProUGUI _finalScoreText;

        [SerializeField] private GameFailMenuSettings _settings;

        #endregion Variables

        #region Unity Functions

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion Unity Functions

        #region Functions

        public void Dispose()
        {
            _retryButton = null;
            _finalScoreText = null;
        }

        private void GetAndSetFinalGeneralScore()
        {
            _finalScoreText.text = Score + RequestGeneralScore().AbbrivateNumber();
        }

        public void Appear()
        {
            transform.localScale = Vector3.zero;
            _retryButton.transform.localScale = Vector3.zero;
            _finalScoreText.transform.localScale = Vector3.zero;

            GetAndSetFinalGeneralScore();

            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence
                .Append(transform.DOScale(1f, _settings.appearDuration))
                .Append(_finalScoreText.transform.DOScale(1f, _settings.appearDuration))
                .Append(_retryButton.transform.DOScale(1f, _settings.appearDuration));

            _sequence.SetDelay(_settings.appearDelay);
        }

        #endregion Functions
    }
}