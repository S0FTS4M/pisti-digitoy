
namespace SOFTSAM.Models.CurrencyManagement
{
    public class MainCurrencyBase : CurrencyBase
    {
        #region Methods

        public MainCurrencyBase(MainCurrencyBase.Settings settings, DataManager dataManager): base(settings, dataManager.PlayerData.currencyData, dataManager)
        {
        }

        #endregion

    }
}
