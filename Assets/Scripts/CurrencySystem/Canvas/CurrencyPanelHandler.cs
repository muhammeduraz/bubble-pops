using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
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

        [SerializeField] private Image _moneyImage;
        [SerializeField] private TextMeshProUGUI _currentAmountText;

        [SerializeField] private double _amountForIcon;
        [SerializeField] private GameObject _moneyIconPrefab;

        #endregion Variables

        #region Unity Functions

        private void Awake()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Terminate();
        }

        #endregion Unity Functions

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
            _currentAmountText.text = "" + currencyAmount.AbbrivateNumber();
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