using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.EnvironmentSystem.Theme.Data;

namespace Assets.Scripts.EnvironmentSystem.Theme
{
    public class ThemeButton : MonoBehaviour, IDisposable
    {
        #region Events

        public Action ThemeChanged;

        #endregion Events
        
        #region Variables

        private int _themeIndex;

        [SerializeField] private Button _themeButton;
        [SerializeField] private ThemeDataSO _themeDataSO;

        #endregion Variables
        
        #region Properties

        public int ThemeIndex { get => _themeIndex; set { if (value == _themeDataSO.ThemeCount) value = 0; _themeIndex = value; } }

        #endregion Properties

        #region Unity Functions

        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion Unity Functions

        #region Functions

        private void Initialize()
        {
            _themeDataSO.Initialize();

            _themeButton.onClick.RemoveAllListeners();
            _themeButton.onClick.AddListener(OnThemeButtonClicked);
        }

        public void Dispose()
        {
            _themeButton.onClick.RemoveAllListeners();

            _themeButton = null;
            _themeDataSO = null;
        }

        private void OnThemeButtonClicked()
        {
            ThemeIndex++;
            _themeDataSO.SetCurrentThemeDataByIndex(ThemeIndex);

            ThemeChanged?.Invoke();
        }

        #endregion Functions
    }

    #region Enums
    
    public enum ThemeType
    {
        Light,
        Dark
    }
    
    #endregion Enums
}