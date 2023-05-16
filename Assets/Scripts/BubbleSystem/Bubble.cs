using TMPro;
using System;
using UnityEngine;
using Assets.Scripts.BubbleSystem.Data;

namespace Assets.Scripts.BubbleSystem
{
    public class Bubble : MonoBehaviour, IDisposable
    {
        #region Variables

        private BubbleData _bubbleData;

        [SerializeField] protected TextMeshPro _idText;
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        #endregion Variables

        #region Properties

        public BubbleData BubbleData { get => _bubbleData; set => _bubbleData = value; }

        #endregion Properties

        #region Unity Functions



        #endregion Unity Functions

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