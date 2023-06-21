using System;
using UnityEngine;

namespace Assets.Scripts.EnvironmentSystem.Theme.Data
{
    [Serializable]
    public class ThemeData
    {
        #region Variables

        [SerializeField] private ThemeType _themeType;

        [SerializeField] private Color _canvasTopColor;
        [SerializeField] private Color _backgroundColor;

        #endregion Variables

        #region Properties

        public ThemeType ThemeType { get => _themeType; }
        
        public Color CanvasTopColor { get => _canvasTopColor; }
        public Color BackgroundColor { get => _backgroundColor; }

        #endregion Properties
    }
}