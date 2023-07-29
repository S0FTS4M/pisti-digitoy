
using System.Collections.Generic;
using SOFTSAM.Models.CurrencyManagement;

public class Player : PlayerBase
{
    public Player(ICurrencyBase currency, DataManager dataManager) : base(currency)
    {
        if(dataManager.PlayerData.IsLoaded)
        {
            WinCount = dataManager.PlayerData.WinCount;
            LoseCount = dataManager.PlayerData.LoseCount;
        }
    }
}
