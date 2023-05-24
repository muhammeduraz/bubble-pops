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

        [SerializeField] private int _randomMaxValue;
        [SerializeField] private List<BubbleData> _bubbleDataList;

        #endregion Variables

        #region Functions

        public BubbleData GetRandomBubbleData()
        {
            return _bubbleDataList[UnityEngine.Random.Range(0, _bubbleDataList.Count)];
        }

        public BubbleData GetRandomBubbleDataByRandomMaxValue()
        {
            return _bubbleDataList[UnityEngine.Random.Range(0, _randomMaxValue)];
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

            int newId = GetMultipliedId(id, count);

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

        public int GetMultipliedId(int id, int count)
        {
            int power = IntegerExtensions.GetPow(id);
            int newId = (int)Math.Pow(2, power + (count - 1));

            return newId;
        }

        #endregion Functions
    }
}