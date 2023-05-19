using System;
using UnityEngine;

namespace Assets.Scripts.BubbleSystem
{
    public class MatchManager : MonoBehaviour, IDisposable
    {
        #region Variables



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

        public void Match(Bubble bubble)
        {

        }

        #endregion Functions
    }
}