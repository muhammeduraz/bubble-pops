using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Extensions.Numeric;

namespace Assets.Scripts.BubbleSystem.Data
{
    [CreateAssetMenu (fileName = "BubbleDataSO", menuName = "Scriptable Objects/Bubble/Data/BubbleDataSO")]
    public class BubbleDataSO : ScriptableObject
    {
        #region Variables

        [SerializeField] private List<BubbleData> _bubbleDataList;

        #endregion Variables

        #region Functions

        public BubbleData GetRandomBubbleData(int maxExclusive = -1)
        {
            if (maxExclusive == -1 || maxExclusive > _bubbleDataList.Count)
                maxExclusive = _bubbleDataList.Count;

            return _bubbleDataList[UnityEngine.Random.Range(10, maxExclusive)];
        }

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

        public BubbleData GetBubbleDataByMultiplication(int id, int count)
        {
            BubbleData bubbleData = null;

            int power = IntegerExtensions.GetPow(id);
            int newId = (int)Math.Pow(2, power + (count - 1));

            for (int i = 0; i < _bubbleDataList.Count; i++)
            {
                bubbleData = _bubbleDataList[i];

                if (bubbleData != null && bubbleData.id == newId)
                {
                    return bubbleData;
                }
            }

            if (bubbleData == null || newId > _bubbleDataList[^1].id)
                bubbleData = _bubbleDataList[^1];

            return bubbleData;
        }

        #endregion Functions
    }
}