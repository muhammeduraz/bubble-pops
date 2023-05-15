using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.CameraSystem
{
    public class CameraService : MonoBehaviour
    {
        #region Variables



        #endregion Variables

        #region Properties



        #endregion Properties

<<<<<<< Updated upstream
        #region Awake - Update - OnDisable
=======
        #region Unity Functions
>>>>>>> Stashed changes

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {

        }

        private void OnDisable()
        {
            Terminate();
        }

<<<<<<< Updated upstream
        #endregion Awake - Update - OnDisable
=======
        #endregion Unity Functions
>>>>>>> Stashed changes

        #region Functions

        private void Initialize()
        {

        }

        private void Terminate()
        {

        }

        #endregion Functions
    }
}