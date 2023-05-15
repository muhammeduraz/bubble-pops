using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.BubbleSystem.Data
{
    [CreateAssetMenu (fileName = "BubbleDataSO", menuName = "Scriptable Objects/Bubble/Data/BubbleDataSO")]
    public class NewScriptableObject : ScriptableObject
    {
        #region Variables

        [SerializeField] private List<BubbleData> _bubbleDataList;

        #endregion Variables

        #region Functions

        public BubbleData GetBubbleDataById(int id)
        {
            BubbleData bubbleData = null;

            for (int i = 0; i < _bubbleDataList.Count; i++)
            {
                bubbleData = _bubbleDataList[i];

                if (bubbleData != null && bubbleData.id == id)
                {
                    return bubbleData;
                }
            }

            return null;
        }

        #endregion Functions
    }
}