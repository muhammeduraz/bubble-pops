using TMPro;
using System;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.ProductSystem;

namespace Assets.Scripts.CanvasSystem.BubbleScoreSystem
{
    public class BubbleScore : MonoBehaviour, IProduct<BubbleScore>
    {
        #region Events

        public Action<BubbleScore> BubbleScoreEnded;

        #endregion Events

        #region Variables

        private Sequence _scoreSequence;

        [SerializeField] private float _movementDuration;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private float _movementAmount;

        [SerializeField] private TextMeshProUGUI _scoreText;

        #endregion Variables
        
        #region Properties
        public Action<BubbleScore> SendToPool { get => BubbleScoreEnded; set => BubbleScoreEnded = value; }

        #endregion Properties

        #region Functions

        public void Appear(Vector3 position)
        {
            gameObject.SetActive(true);
            transform.position = position;

            _scoreSequence?.Kill();
            _scoreSequence = DOTween.Sequence();
            _scoreSequence
                .Append(transform.DOMoveY(transform.position.y + _movementAmount, _movementDuration))
                .Join(_scoreText.DOFade(0f, _fadeDuration));

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

        #endregion Functions
    }
}