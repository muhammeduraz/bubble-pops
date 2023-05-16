using System;
using UnityEngine;
using Assets.Scripts.BubbleSystem;

namespace Assets.Scripts.BubbleSystem
{
    public class BubbleManager : MonoBehaviour, IDisposable
    {
        #region Variables

        private Bubble

        [SerializeField] private Bubble _bubblePrefab;

        #endregion Variables

        #region Properties



        #endregion Properties

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

        }

        public void Dispose()
        {

        }

        #endregion Functions
    }
}