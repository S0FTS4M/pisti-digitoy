using SOFTSAM.Models.CurrencyManagement;

[System.Serializable]
public class PlayerData : ISaveLoadData
{
    public bool IsLoaded { get; set; }
    public CurrencyBase.CurrencyData currencyData;
    public int WinCount;
    public int LoseCount;

    public PlayerData()
    {
        currencyData = new CurrencyBase.CurrencyData() { IsLoaded = false };
    }

    public void SetLoaded(bool isLoaded)
    {
        currencyData.IsLoaded = isLoaded;
    }
}