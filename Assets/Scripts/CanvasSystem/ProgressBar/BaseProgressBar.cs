using TMPro;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Assets.Scripts.CanvasSystem.ProgressBar
{
    public abstract class BaseProgressBar : MonoBehaviour, IDisposable
    {
        #region Variables

        private const string ProgressTextPrefix = "%";

        private bool _isProgressTextActive;

        protected Tween fillTween;
        protected Tween scaleTween;

        [SerializeField] protected float fillDuration;

        [SerializeField] protected Ease scaleInEase;
        [SerializeField] protected Ease scaleOutEase;

        [SerializeField] protected float fadeDuration;

        [SerializeField] protected Image fillImage;
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected TextMeshProUGUI _progressText;

        #endregion Variables

        #region Unity Functions

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion Unity Functions

        #region Functions

        public virtual void Dispose()
        {

        }

        public virtual void Appear(bool withProgressText)
        {
            Reset();

            _isProgressTextActive = withProgressText;
            _progressText.gameObject.SetActive(withProgressText);

            Fade(1f);
        }

        public virtual void Disappear()
        {
            Fade(0f);
        }

        private void Fade(float value)
        {
            canvasGroup.DOFade(value, fadeDuration);
        }

        public void SetValue(float targetValue)
        {
            fillTween?.Kill();
            fillTween = fillImage.DOFillAmount(targetValue, fillDuration);
            fillTween.SetEase(Ease.Linear);

            if (_isProgressTextActive)
            {
                fillTween.OnUpdate(() =>
                {
                    _progressText.text = ProgressTextPrefix + ((int)(fillImage.fillAmount * 100f));
                });
            }
        }

        public void SetValue(float targetValue, float duration = 0.25f)
        {
            fillTween?.Kill();
            fillTween = fillImage.DOFillAmount(targetValue, duration);
            fillTween.SetEase(Ease.Linear);

            if (_isProgressTextActive)
            {
                fillTween.OnUpdate(() =>
                {
                    _progressText.text = ProgressTextPrefix + ((int)(fillImage.fillAmount * 100f));
                });
            }
        }

        public void Reset()
        {
            fillTween?.Kill();
            scaleTween?.Kill();

            fillImage.fillAmount = 0f;
            canvasGroup.alpha = 0f;
        }

        #endregion Functions
    }
}