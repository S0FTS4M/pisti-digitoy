using System;

namespace SOFTSAM.Models.CurrencyManagement
{
    public interface ICurrencyBase
    {
        double Amount { get; }
        string Name { get; }

        event EventHandler<CurrencyEventArgs<double>> CurrencyChanged;

        bool CanAfford(double amount);
        void SetAmount(double amount);
        
        void Increase(double increaseAmount);
        void Decrease(double decreaseAmount);
        void NotifyIfCurrencyChanged();
        void ResetToDefaults();
    }
}
