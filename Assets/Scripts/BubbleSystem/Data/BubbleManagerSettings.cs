using UnityEngine;

namespace Assets.Scripts.BubbleSystem.Data
{
    [CreateAssetMenu (fileName = "BubbleManagerSettings", menuName = "Scriptable Objects/BubbleSystem/Data/BubbleManagerSettings")]
    public class BubbleManagerSettings : ScriptableObject
    {
        #region Variables

        public int minActiveBubbleToCreateNewLine;

        #endregion Variables
    }
}