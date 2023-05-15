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

        #region Unity Functions

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

        #endregion Unity Functions

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