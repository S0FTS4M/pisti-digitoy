using SOFTSAM.Models.CurrencyManagement;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public Deck.Settings deckSettings;
    public RoomManager.Settings roomManagerSettings;
    public CurrencyBase.Settings currencyBaseSettings;
    public BotController.Settings botSettings;
    public CardView.Settings cardViewSettings;
    public Card.Settings cardSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(roomManagerSettings);
        Container.BindInstance(deckSettings);
        Container.BindInstance(currencyBaseSettings);
        Container.BindInstance(botSettings);
        Container.BindInstance(cardViewSettings);
        Container.BindInstance(cardSettings);
    }
}
