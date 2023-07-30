using System.Collections;
using System.Collections.Generic;
using SOFTSAM.Models.CurrencyManagement;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject]
    Deck.Settings deckSettings;

    [Inject]
    RoomManager.Settings roomManagerSettings;

    [Inject]
    BotController.Settings botSettings;

    [Inject]
    CardView.Settings cardViewSettings;

    public override void InstallBindings()
    {
        Container.Bind<DataManager>().AsSingle();
        Container.Bind<RoomManager>().AsSingle();
        Container.Bind<PlayerBase>().To<Player>().AsSingle();
        Container.Bind<PlayerControllerBase>().To<PlayerController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ICurrencyBase>().To<MainCurrencyBase>().AsTransient();
        Container.Bind<IDeck>().To<Deck>().AsTransient();
        Container.Bind<IList<ICard>>().FromMethod(CreateCards).AsTransient();
        Container.Bind<EntranceUIController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<DeckController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<TableController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ITurnManager>().To<TurnManager>().AsSingle();
        Container.Bind<GameRuleManager>().AsSingle().NonLazy();
        Container.BindFactory<RoomUIController, RoomUIController.Factory>().FromComponentInNewPrefab(roomManagerSettings.RoomPrefab);
        Container.BindFactory<BotController, BotController.Factory>().FromComponentInNewPrefab(botSettings.BotPrefab);
        Container.BindFactory<Bot, Bot.Factory>().AsSingle();
        Container.BindFactory<Deck, Deck.Factory>().AsSingle();
        Container.BindMemoryPool<CardView, CardView.CardPool>().FromComponentInNewPrefab(cardViewSettings.cardPrefab);
    }

    private List<ICard> CreateCards()
    {
        var cards = new List<ICard>();
        for (int i = 0; i < deckSettings.cardInfos.Count; i++)
        {
            var cardInfo = deckSettings.cardInfos[i];
            var card = new Card(cardInfo.rank, cardInfo.suit, cardInfo.cardImage);
            cards.Add(card);
        }

        return cards;
    }
}
