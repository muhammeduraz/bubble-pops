using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Assets.Scripts.CanvasSystem.ProgressBar;

namespace Assets.Scripts.CanvasSystem.Loading
{
    public class LoadingPanel : MonoBehaviour, IDisposable
    {
        #region Variables

        [SerializeField] private float _appearDuration;
        [SerializeField] private float _disappearDuration;

        [SerializeField] private Image _backgroundImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private DefaultProgressBar _progressBar;

        #endregion Variables

        #region Properties

        public float AppearDuration { get => _appearDuration; }
        public float DisappearDuration { get => _disappearDuration; }

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

        }

        public void Dispose()
        {

        }

        public void Appear()
        {
            Reset();

            _progressBar.Appear(true);
            _canvasGroup.DOFade(1f, _appearDuration);
        }

        public void Disappear()
        {
            _canvasGroup.DOFade(0f, _disappearDuration)
                .OnComplete(()=> gameObject.SetActive(false));
        }

        private void Reset()
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = 0f;
            _progressBar.SetValue(0f, 0f);

        }

        public void UpdateProgress(float progress, float duration = 0.25f)
        {
            _progressBar.SetValue(progress, duration);
        }

        #endregion Functions
    }
}