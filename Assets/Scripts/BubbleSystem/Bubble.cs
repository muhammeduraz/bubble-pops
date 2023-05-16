using TMPro;
using System;
using UnityEngine;
using Assets.Scripts.ProductSystem;
using Assets.Scripts.BubbleSystem.Data;

namespace Assets.Scripts.BubbleSystem
{
    public class Bubble : MonoBehaviour, IProduct<Bubble>
    {
        #region Events

        public Action<Bubble> SendToPoolEvent;
        
        #endregion Events

        #region Variables

        private BubbleData _bubbleData;

        [SerializeField] protected TextMeshPro _idText;
        [SerializeField] protected TrailRenderer _trailRenderer;
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        #endregion Variables

        #region Properties

        public BubbleData BubbleData { get => _bubbleData; set => _bubbleData = value; }
        public Action<Bubble> SendToPool { get => SendToPoolEvent; set => SendToPoolEvent = value; }

        #endregion Properties

        #region Functions

        public void Initialize()
        {
            UpdateBubble();
        }

        public void Dispose()
        {

        }

        public void UpdateBubble()
        {
            SetText("" + _bubbleData.id);
            SetColor(_bubbleData.color);
        }

        public void UpdateBubble(BubbleData bubbleData)
        {
            _bubbleData = bubbleData;
            UpdateBubble();
        }

        private void SetText(string text)
        {
            _idText.text = text;
        }

        private void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }

        #endregion Functions
    }
}