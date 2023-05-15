using System;
using UnityEngine;

namespace Assets.Scripts.PlayerSystem.Animation
{
    public class PlayerAnimationHandler : IDisposable
    {
        #region Variables

<<<<<<< Updated upstream
        private int RunHash;
=======
        private readonly int RunHash = Animator.StringToHash("Run");
>>>>>>> Stashed changes

        private Animator _animator;

        #endregion Variables

        #region Properties



        #endregion Properties

        #region Functions

        public PlayerAnimationHandler(Animator animator)
        {
            _animator = animator;
        }

        public void Initialize()
        {
<<<<<<< Updated upstream
            RunHash = Animator.StringToHash("Run");

=======
>>>>>>> Stashed changes
            SubscribeSignals(true);
        }

        public void Dispose()
        {
            SubscribeSignals(false);

            _animator = null;
        }

        private void SubscribeSignals(bool subscribe)
        {
            if (subscribe)
            {
                
            }
            else if (!subscribe)
            {
                
            }
        }

        private void OnPlayButtonClickedSignal()
        {
            StartRunAnimation();
        }

        public void StartIdleAnimation()
        {
            
        }

        public void StartRunAnimation()
        {
            _animator.SetBool(RunHash, true);
        }

        #endregion Functions
    }
}