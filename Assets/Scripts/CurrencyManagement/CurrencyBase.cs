using System;
using UnityEngine;
using Zenject;

namespace SOFTSAM.Models.CurrencyManagement
{

    //[JsonObject(MemberSerialization.OptIn)]
    public class CurrencyBase : ICurrencyBase, ITickable
    {
        #region Variables

        private bool _isCurrencyChanged;

        private double _deltaIncrease;

        private double _deltaDecrease;

        private DataManager _dataManager;

        private CurrencyData _currencyData;

        #endregion

        #region Props

        //[JsonProperty]
        public double Amount { get; private set; }

        public string Name { get; private set; }

        #endregion

        #region Events

        public event EventHandler<CurrencyEventArgs<double>> CurrencyChanged;

        #endregion

        #region Methods

        public CurrencyBase(Settings settings, CurrencyData currencyData, DataManager dataManager)
        {
            _dataManager = dataManager;
            _currencyData = currencyData;

            if (currencyData.IsLoaded == false)
                Amount = settings.Value;
            else
                Amount = currencyData.Amount;

            SaveCurrency();

            _isCurrencyChanged = true;
        }

        public void SetAmount(double amount)
        {
            Amount = amount;

            _isCurrencyChanged = true;
        }

        public void Increase(double increaseAmount)
        {
            Amount += increaseAmount;

            _isCurrencyChanged = true;
            _deltaIncrease += increaseAmount;

            SaveCurrency();
        }

        private void SaveCurrency()
        {
            _currencyData.Amount = Amount;
            _dataManager.SavePlayerData();
        }

        public void Decrease(double decreaseAmount)
        {
            if (CanAfford(decreaseAmount) == false) return;

            Amount -= decreaseAmount;

            _isCurrencyChanged = true;
            _deltaDecrease -= decreaseAmount;

            SaveCurrency();
        }

        public bool CanAfford(double amount)
        {
            return amount <= Amount;
        }

        public void NotifyIfCurrencyChanged()
        {
            if (_isCurrencyChanged)
            {
                var eventArgs = new CurrencyEventArgs<double>
                {
                    DeltaIncrease = _deltaIncrease,
                    DeltaDecrease = _deltaDecrease
                };

                CurrencyChanged?.Invoke(this, eventArgs);

                _isCurrencyChanged = false;
                _deltaIncrease = 0;
                _deltaDecrease = 0;
            }
        }

        public void ResetToDefaults()
        {
            Amount = 0;
        }

        public void Tick()
        {
            NotifyIfCurrencyChanged();
        }

        #endregion

        [System.Serializable]
        public class Settings
        {
            public double Value;
        }

        [System.Serializable]
        public class CurrencyData : ISaveLoadData
        {
            public double Amount;

            public bool IsLoaded { get; set; }
        }
    }
}
