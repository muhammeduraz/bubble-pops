using System;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.CanvasSystem.Score
{
    public class ScorePanel : MonoBehaviour, IDisposable
    {
        #region Variables

        [SerializeField] private float _fadeDuration;
        [SerializeField] private CanvasGroup _canvasGroup;

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
            _canvasGroup = null;
        }

        public void Disappear()
        {
            _canvasGroup.DOFade(0f, _fadeDuration);
        }

        #endregion Functions
    }
}