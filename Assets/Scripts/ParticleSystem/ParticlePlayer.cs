using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.BubbleSystem.Data;

namespace Assets.Scripts.Particle
{
    public class ParticlePlayer : MonoBehaviour, IDisposable
    {
        #region Variables

        [SerializeField] private BubbleDataSO _bubbleDataSO;
        [SerializeField] private BubbleParticle _bubbleParticlePrefab;

        private Stack<BubbleParticle> _particleStack;

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
            _particleStack = new Stack<BubbleParticle>();
        }

        public void Dispose()
        {
            _bubbleParticlePrefab = null;
            _particleStack = null;
        }

        private void AddBubbleParticleToStack(BubbleParticle bubbleParticle)
        {
            bubbleParticle.ParticleEnded -= AddBubbleParticleToStack;
            _particleStack.Push(bubbleParticle);
        }

        public void PlayParticle(int id, Vector3 position)
        {
            BubbleParticle bubbleParticle = GetBubbleParticle();
            bubbleParticle.SetPosition(position);

            Color color = GetColorById(id);
            bubbleParticle.SetColor(color);
            
            bubbleParticle.ParticleEnded += AddBubbleParticleToStack;
            bubbleParticle.Initialize();
        }

        private Color GetColorById(int id)
        {
            BubbleData bubbleData = _bubbleDataSO.GetBubbleDataById(id);
            if (bubbleData != null)
                return bubbleData.color;

            return Color.white;
        }

        private BubbleParticle GetBubbleParticle()
        {
            BubbleParticle bubbleParticle = GetBubbleParticleFromPool();

            if (bubbleParticle == null)
            {
                bubbleParticle = CreateBubbleParticle();
            }

            return bubbleParticle;
        }

        private BubbleParticle GetBubbleParticleFromPool()
        {
            if (_particleStack.Count <= 0) return null;

            _particleStack.TryPop(out BubbleParticle bubbleParticle);
            return bubbleParticle;
        }

        private BubbleParticle CreateBubbleParticle()
        {
            return Instantiate(_bubbleParticlePrefab, transform, true);
        }

        #endregion Functions
    }
}