using System.Collections.Generic;
using DG.Tweening;
using SOFTSAM.Models.CurrencyManagement;
using Zenject;

public class Bot : PlayerBase
{
    public Bot(ICurrencyBase currencyBase) : base(currencyBase)
    {
  
    }

    public class Factory : PlaceholderFactory<Bot>
    {

    }

}
