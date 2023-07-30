using SOFTSAM.Models.CurrencyManagement;
using TMPro;
using UnityEngine;
using Zenject;

public class EntranceUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject roomContainer;

    [SerializeField]
    private TextMeshProUGUI currencyText;

    private PlayerBase player;

    [Inject]
    private void Construct(PlayerBase playerBase)
    {
        player = playerBase;
        currencyText.SetText(playerBase.Currency.Amount.ToString());

        playerBase.Currency.CurrencyChanged += OnCurrencyChanged;
    }

    private void OnCurrencyChanged(object sender, CurrencyEventArgs<double> e)
    {
        Debug.Log("hello");
        currencyText.SetText(((CurrencyBase)sender).Amount.ToString());
    }

    public void Show()
    {
        roomContainer.SetActive(true);
        currencyText.SetText(player.Currency.Amount.ToString());
    }
    public void Hide()
    {
        roomContainer.SetActive(false);

    }
}
