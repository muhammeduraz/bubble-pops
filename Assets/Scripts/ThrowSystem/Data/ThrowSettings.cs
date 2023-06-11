using UnityEngine;

namespace Assets.Scripts.ThrowSystem.Data
{
    [CreateAssetMenu (fileName = "ThrowSettings", menuName = "Scriptable Objects/BubbleSystem/Data/ThrowSettings")]
    public class ThrowSettings : ScriptableObject
    {
        #region Variables

        public float distance;
        public float duration;
        public float minDuration;
        public float maxDuration;

        #endregion Variables

        #region Functions

        public float GetThrowDuration(float currentDistance)
        {
            float throwDuration = (currentDistance * duration) / distance;
            throwDuration = Mathf.Clamp(throwDuration, minDuration, maxDuration);

            return throwDuration;
        }

        #endregion Functions
    }
}