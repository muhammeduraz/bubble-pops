using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.EnvironmentSystem.Theme.Data
{
    [CreateAssetMenu (fileName = "ThemeDataSO", menuName = "Scriptable Objects/Theme/ThemeDataSO")]
    public class ThemeDataSO : ScriptableObject
    {
        #region Variables

        private ThemeData _currentThemeData;

        [SerializeField] private List<ThemeData> _themeDataList;

        #endregion Variables
        
        #region Properties

        public int ThemeCount { get => _themeDataList.Count; }
        public ThemeData CurrentThemeData { get => _currentThemeData; }

        #endregion Properties

        #region Functions

        public void Initialize()
        {
            SetCurrentThemeDataByIndex(0);
        }

        public void SetCurrentThemeDataByIndex(int index)
        {
            _currentThemeData = _themeDataList[index % _themeDataList.Count];
        }

        #endregion Functions
    }
}