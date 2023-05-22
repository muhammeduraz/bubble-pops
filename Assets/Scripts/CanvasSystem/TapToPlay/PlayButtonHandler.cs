using TMPro;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Assets.Scripts.CanvasSystem
{
    public class PlayButtonHandler : MonoBehaviour, IDisposable
    {
        #region Variables

        private Sequence _textSequence;

        [SerializeField] private Button _playButton;
        [SerializeField] private TextMeshProUGUI _playText;

        #endregion Variables

        #region Unity Functions

        private void Awake()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Terminate();
            Dispose();
        }

        #endregion Unity Functions

        #region Functions

        public void PlayButtonHandlerConstruct()
        {
            
        }

        private void Initialize()
        {
            _playButton.onClick.RemoveAllListeners();
            _playButton.onClick.AddListener(OnPlayButtonClicked);

            ActivatePlayTextAnimation();
        }

        private void Terminate()
        {
            _playButton.onClick.RemoveAllListeners();

            DeactivatePlayTextAnimation();
        }

        public void Dispose()
        {
            _playButton = null;
        }

        private void OnPlayButtonClicked()
        {
            gameObject.SetActive(false);
        }

        private void ActivatePlayTextAnimation()
        {
            _textSequence?.Kill();
            _textSequence = DOTween.Sequence();
            _textSequence
                .Join(_playText.transform.DOScale(1.25f, 0.5f))
                .Join(_playText.DOFade(0.75f, 0.5f))
                .Append(_playText.transform.DOScale(1f, 0.5f))
                .Join(_playText.DOFade(1f, 0.5f));

            _textSequence.SetLoops(-1, LoopType.Restart);
        }

        private void DeactivatePlayTextAnimation()
        {
            _textSequence?.Kill();
        }

        #endregion Functions
    }
}