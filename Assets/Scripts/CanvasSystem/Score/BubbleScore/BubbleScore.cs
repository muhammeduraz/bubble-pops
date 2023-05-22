using TMPro;
using System;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.ProductSystem;

namespace Assets.Scripts.CanvasSystem.Score.BubbleScore
{
    public class BubbleScore : MonoBehaviour, IProduct<BubbleScore>
    {
        #region Events

        public Action<BubbleScore> BubbleScoreEnded;

        #endregion Events

        #region Variables

        private Sequence _scoreSequence;

        [SerializeField] private float _movementAmount;
        [SerializeField] private float _movementDuration;

        [SerializeField] private float _fadeDelay;
        [SerializeField] private float _fadeDuration;

        [SerializeField] private TextMeshProUGUI _scoreText;

        #endregion Variables
        
        #region Properties

        public Action<BubbleScore> SendToPool { get => BubbleScoreEnded; set => BubbleScoreEnded = value; }

        #endregion Properties

        #region Functions

        public void Appear(int id, Vector3 position)
        {
            SetText(id);
            transform.position = position;

            gameObject.SetActive(true);

            _scoreSequence?.Kill();
            _scoreSequence = DOTween.Sequence();
            _scoreSequence
                .Append(transform.DOMoveY(transform.position.y + _movementAmount, _movementDuration))
                .Join(_scoreText.DOFade(0f, _fadeDuration).SetDelay(_fadeDelay));

            _scoreSequence.OnComplete(() =>
            {
                Dispose();
            });
        }

        public void Dispose()
        {
            _scoreText.alpha = 1f;
            BubbleScoreEnded?.Invoke(this);

            gameObject.SetActive(false);
        }

        private void SetText(int id)
        {
            _scoreText.text = "" + id;
        }

        #endregion Functions
    }
}