using UnityEngine;

namespace Assets.Scripts.BubbleSystem.Creator
{
    [CreateAssetMenu (fileName = "BubbleCreatorSettings", menuName = "Scriptable Objects/BubbleSystem/BubbleCreatorSettings")]
    public class BubbleCreatorSettings : ScriptableObject
    {
        #region Variables

        public Bubble bubblePrefab;

        public float verticalOffset;
        public float horizontalOffset;
        public Vector3 initialSpawnPosition;

        public int lineSize;
        public int initialLineCount;

        #endregion Variables
    }
}