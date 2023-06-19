using UnityEngine;

namespace Assets.Scripts.CanvasSystem.Menus.Data
{
    [CreateAssetMenu (fileName = "GameFailMenuSettings", menuName = "Scriptable Objects/Canvas/Menu/GameFailMenuSettings")]
    public class GameFailMenuSettings : ScriptableObject
    {
        #region Variables

        public float appearDelay;
        public float appearDuration;

        #endregion Variables
    }
}