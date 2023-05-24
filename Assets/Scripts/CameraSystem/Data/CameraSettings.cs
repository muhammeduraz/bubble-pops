using System;
using UnityEngine;

namespace Assets.Scripts.CameraSystem.Data
{
    [CreateAssetMenu (fileName = "CameraSettings", menuName = "Scriptable Objects/Camera/CameraSettings")]
    public class CameraSettings : ScriptableObject
    {
        #region Variables

        public float shakeDuration;
        public float shakeStrength;
        public int shakeVibrato;

        #endregion Variables
    }
}