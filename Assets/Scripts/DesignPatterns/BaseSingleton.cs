using System;
using UnityEngine;

namespace Assets.Scripts.DesignPatterns
{
    public abstract class BaseSingleton<T> : MonoBehaviour, IInitializable, IDisposable where T : BaseSingleton<T>
    {
        #region Variables

        protected static T instance;

        #endregion Variables

        #region Properties

        protected static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(T)) as T;

                    if (instance == null)
                    {
                        GameObject baseSingletonGameObject = new GameObject($"{typeof(T).Name}_Singleton");
                        instance = baseSingletonGameObject.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        #endregion Properties

<<<<<<< Updated upstream
        #region Awake - OnDisable
=======
        #region Unity Functions
>>>>>>> Stashed changes

        private void Awake()
        {
            InitializeSingleton();
        }

        private void OnDisable()
        {
            DisposeSingleton();
        }

<<<<<<< Updated upstream
        #endregion Awake - OnDisable
=======
        #endregion Unity Functions
>>>>>>> Stashed changes

        #region Functions

        private void InitializeSingleton()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else if (instance != null)
            {
                DestroyImmediate(this);
            }

            Initialize();
        }

        private void DisposeSingleton()
        {
            instance = null;

            Dispose();
        }

        public abstract void Initialize();
        public abstract void Dispose();

        #endregion Functions
    }
}