using System;
using UnityEngine;

namespace Assets.Scripts.GameSystem.States
{
    public class BasePlayerState : ScriptableObject, IGameState, IDisposable
    {
        #region Variables



        #endregion Variables

        #region Functions

        public void Initialize()
        {
            
        }

        public virtual void OnStateEnter()
        {

        }

        public virtual void OnStateExit()
        {

        }

        public virtual void Dispose()
        {

        }

        #endregion Functions
    }
}