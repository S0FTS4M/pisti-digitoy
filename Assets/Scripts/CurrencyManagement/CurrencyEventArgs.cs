using System;

namespace SOFTSAM.Models.CurrencyManagement
{
    public class CurrencyEventArgs<T> : EventArgs
    {
        public T DeltaIncrease;
        public T DeltaDecrease;
    }
}