using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.CanvasSystem.Canvases
{
    public class GameCanvasHandler : MonoBehaviour, IDisposable
    {
        #region Editor Variables

#if UNITY_EDITOR

        [SerializeField] private EventSystem _eventSystemPrefab;

#endif

        #endregion Editor Variables

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
#if UNITY_EDITOR

            CheckEventSystem();

#endif
        }

        public void Dispose()
        {

        }

        #endregion Functions

        #region Editor Functions

#if UNITY_EDITOR

        private void CheckEventSystem()
        {
            if (EventSystem.current == null)
            {
                Instantiate(_eventSystemPrefab, transform);
            }
        }

#endif

        #endregion Editor Functions
    }
}