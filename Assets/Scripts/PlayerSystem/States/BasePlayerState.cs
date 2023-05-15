using System;
using UnityEngine;

namespace Assets.Scripts.PlayerSystem.States
{
    public class BasePlayerState : ScriptableObject, IPlayerState, IDisposable
    {
        #region Variables



        #endregion Variables

        #region Functions

        public virtual void StateInitialize()
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