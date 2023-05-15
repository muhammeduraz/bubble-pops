using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.RaycastSystem;

namespace Assets.Scripts.CurrencySystem
{
    public class CurrencyService : IDisposable
    {
        #region Variables

        private const string CurrencySaveKey = "CurrencySaveKey";

        private double _targetAmount;
        private double _currencyAmount;

        private Tween _amountTween;
        private SaveService _saveService;

        #endregion Variables

        #region Properties



        #endregion Properties

        #region Functions

        public CurrencyService(SaveService saveService)
        {
            _saveService = saveService;
        }

        public void Initialize()
        {
            
        }

        public void Dispose()
        {

        }

        public void AddCurrency(double amount, bool isFlow)
        {
            _currencyAmount += amount;
        }

        private void SaveCurrencyAmount()
        {
            _saveService.Save(CurrencySaveKey, _currencyAmount);
        }

        private void LoadCurrencyAmount()
        {
            _currencyAmount = _saveService.Load<double>(CurrencySaveKey);
        }

        #endregion Functions
    }
}