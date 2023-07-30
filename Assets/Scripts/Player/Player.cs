
using System.Collections.Generic;
using SOFTSAM.Models.CurrencyManagement;

public class Player : PlayerBase
{
    private DataManager _dataManager;
    public Player(ICurrencyBase currency, DataManager dataManager) : base(currency)
    {
        _dataManager = dataManager;
        name = "Player";
        if(dataManager.PlayerData.IsLoaded)
        {
            WinCount = dataManager.PlayerData.WinCount;
            LoseCount = dataManager.PlayerData.LoseCount;
        }
    }

    public override void Win()
    {
        base.Win();
        _dataManager.PlayerData.WinCount = WinCount;
        _dataManager.PlayerData.LoseCount = LoseCount;
        _dataManager.SavePlayerData();
    }
    public override void Lose()
    {
        base.Lose();
        _dataManager.PlayerData.WinCount = WinCount;
        _dataManager.PlayerData.LoseCount = LoseCount;
        _dataManager.SavePlayerData();
    }
}
