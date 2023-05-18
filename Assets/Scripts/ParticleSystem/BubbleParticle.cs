using System;
using UnityEngine;

namespace Assets.Scripts.Particle
{
    public class BubbleParticle : MonoBehaviour
    {
        #region Events

        public Action<BubbleParticle> ParticleEnded;

        #endregion Events

        #region Variables

        [SerializeField] private ParticleSystem _particleSystem;

        #endregion Variables

        #region Unity Functions

        private void Update()
        {
            OnUpdate();
        }

        #endregion Unity Functions

        #region Functions

        public void Initialize()
        {
            gameObject.SetActive(true);
            _particleSystem.Play(true);
        }

        public void SetColor(Color color)
        {
            ParticleSystem.MainModule main = _particleSystem.main;
            main.startColor = color;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void OnUpdate()
        {
            if (_particleSystem.isPlaying) return;

            gameObject.SetActive(false);
            ParticleEnded?.Invoke(this);
        }

        #endregion Functions
    }
}