using System;
using UnityEngine;

namespace Assets.Scripts.BubbleSystem.Data
{
    [Serializable]
    public class BubbleData
    {
        #region Variables

        public int id;
        public Color color;

        #endregion Variables

        #region Functions

        public BubbleData(int id, Color color)
        {
            this.id = id;
            this.color = color;
        }

        ~BubbleData()
        {
            
        }

        #endregion Functions
    }
}