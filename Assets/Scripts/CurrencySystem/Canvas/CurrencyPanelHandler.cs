using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Assets.Scripts.RaycastSystem;
using Assets.Scripts.Extensions.Numeric;

namespace Assets.Scripts.CurrencySystem.Canvas
{
    public class CurrencyPanelHandler : MonoBehaviour
    {
        #region Variables

        private RaycastOperations _raycastService;

        private Tween _amountTween;

        private double _targetAmount;
        private double _currencyAmount;

        [BoxGroup("Components")][SerializeField] private Image _moneyImage;
        [BoxGroup("Components")][SerializeField] private TextMeshProUGUI _currentAmountText;

        [BoxGroup("Flow Settings")][SerializeField] private double _amountForIcon;
        [BoxGroup("Flow Settings")][SerializeField] private GameObject _moneyIconPrefab;

        #endregion Variables

        #region Awake - OnDisable

        private void Awake()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Terminate();
        }

        #endregion Awake - OnDisable

        #region Functions

        private void Initialize()
        {
            SubscribeEvents(true);
        }

        private void Terminate()
        {
            SubscribeEvents(false);
        }

        private void SubscribeEvents(bool subscribe)
        {
            if (subscribe)
            {
                
            }
            else if (!subscribe)
            {
                
            }
        }

        private void OnCurrencyAmountChanged(double currentCurrencyAmount, bool isFlow)
        {
            _currencyAmount = currentCurrencyAmount;

            if (isFlow)
            {
                MoneyFlow(new Vector2(Screen.width / 2f, Screen.height / 2f));
            }
            else if (!isFlow)
            {
                AddCurrencyWithTween();
            }
        }

        private void UpdateCurrencyText(double currencyAmount)
        {
            _currentAmountText.text = "" + currencyAmount.AbbrivateNum();
        }

        private void AddCurrencyWithTween(float duration = 1f)
        {
            _amountTween?.Kill();
            _amountTween = DOTween.To(() => _targetAmount, x => _targetAmount = x, _currencyAmount, duration)
                .OnUpdate(() =>
                {
                    UpdateCurrencyText(_targetAmount);
                });
        }

        private void MoneyFlow(Vector2 screenPoint)
        {
            //Vector2 objectPosition = _raycastService.GetWorldToScreenPoint(moneyWorldPosition);

            GameObject go = Instantiate(_moneyIconPrefab, transform);
            go.transform.position = screenPoint;

            go.transform.DOScale(1f, 0.2f);
            go.transform.DOMove(_moneyImage.transform.position, 1f)
                .OnComplete(()=>
                {
                    AddCurrencyWithTween();
                });
        }

        #endregion Functions
    }
}