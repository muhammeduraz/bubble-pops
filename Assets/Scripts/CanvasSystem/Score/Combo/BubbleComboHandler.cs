using TMPro;
using System;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.CanvasSystem.Score.Combo
{
    public class BubbleComboHandler : MonoBehaviour, IDisposable
    {
        #region Variables

        private const string ComboPostfix = "X";

        private Sequence _sequence;

        [SerializeField] private float _scaleAmount;
        [SerializeField] private float _scaleDuration;
        [SerializeField] private Ease _scaleEase;

        [SerializeField] private float _fadeDelay;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private Ease _fadeEase;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _comboText;

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

        }

        public void Dispose()
        {
            _sequence?.Kill();
            _sequence = null;

            _comboText = null;
        }

        private void SetText(int combo)
        {
            _comboText.text = combo + ComboPostfix;
        }

        private void PlayAnimation()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_comboText.transform.DOScale(_scaleAmount, _scaleDuration).SetEase(_scaleEase))
                .Join(_canvasGroup.DOFade(0f, _fadeDuration).SetEase(_fadeEase).SetDelay(_fadeDelay));
        }

        public void ShowCombo(int combo)
        {
            _canvasGroup.alpha = 1f;
            _comboText.transform.localScale = Vector3.zero;

            SetText(combo);
            PlayAnimation();
        }

        #endregion Functions
    }
}