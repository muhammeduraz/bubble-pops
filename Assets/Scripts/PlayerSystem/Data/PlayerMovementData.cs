using UnityEngine;

namespace Assets.Scripts.PlayerSystem.Data
{
    [CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Scriptable Objects/Player/Data/PlayerMovementData")]
    public class PlayerMovementData : ScriptableObject
    {
        #region Variables

        [SerializeField] private float _verticalSpeed;
        [SerializeField] private float _horizontalSpeed;

        [SerializeField] private float _verticalSpeedLerpMultiplier;
        [SerializeField] private float _horizontalSpeedLerpMultiplier;

        #endregion Variables

        #region Properties

        public float VerticalSpeed { get => _verticalSpeed; set => _verticalSpeed = value; }
        public float HorizontalSpeed { get => _horizontalSpeed; set => _horizontalSpeed = value; }

        public float VerticalSpeedLerpMultiplier { get => _verticalSpeedLerpMultiplier; set => _verticalSpeedLerpMultiplier = value; }
        public float HorizontalSpeedLerpMultiplier { get => _horizontalSpeedLerpMultiplier; set => _horizontalSpeedLerpMultiplier = value; }

        #endregion Properties

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