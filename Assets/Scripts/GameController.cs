using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private GameObject _container;

    [SerializeField]
    private Transform _botParent;

    private RoomManager _roomManager;

    private BotController.Factory _botFactory;

    private Dictionary<IPlayer, PlayerControllerBase> _playerControllers = new Dictionary<IPlayer, PlayerControllerBase>();

    private CardView.CardPool _cardPool;

    private DeckController _deckController;

    private IPlayer _currentPlayer;

    private Deck.Settings _deckSettings;

    private TableController _tableController;

    [Inject]
    public void Construct(RoomManager roomManager, BotController.Factory botFactory, IPlayer player, PlayerControllerBase playerController, CardView.CardPool cardPool, DeckController deckController, Deck.Settings deckSettings, TableController tableController)
    {
        _roomManager = roomManager;
        _botFactory = botFactory;
        _cardPool = cardPool;
        _deckController = deckController;
        _currentPlayer = player;
        _deckSettings = deckSettings;
        _tableController = tableController;

        _roomManager.RoomReady += OnRoomReady;

        _playerControllers.Add(player, playerController);
    }

    private void OnRoomReady(Room room)
    {
        _container.gameObject.SetActive(true);

        _tableController.Initialize(_deckController.Deck);

        PutCardsOnTable(_deckController.Deck, 2, 1);

        for (int i = 0; i < room.Players.Count; i++)
        {
            var player = room.Players[i];
            player.PlayerDrawnCards += OnPlayerDrawnCards;
        }

        //create bots here
        for (int i = 1; i < room.Players.Count; i++)
        {
            var bot = _botFactory.Create();
            bot.SetPlayer(room.Players[i]);
            bot.transform.SetParent(_botParent);
            _playerControllers.Add(room.Players[i], bot);
        }

        var seq = DOTween.Sequence();

        for (int cardCount = 0; cardCount < 4; cardCount++)
        {
            for (int i = 0; i < room.Players.Count; i++)
            {
                var player = room.Players[i];
                seq.AppendCallback(() => player.DrawCardsFromDeck(1));
                seq.AppendInterval(_deckSettings.drawAnimDelay);
            }
        }

    }

    private void PutCardsOnTable(Deck deck, int unvisibleCardCount, int visibleCardCount)
    {
        var seq = DOTween.Sequence();
        for (int i = 0; i < unvisibleCardCount; i++)
        {
            var card = deck.DrawCard();
            CreateCardView(card, out CardView cardView, out RectTransform cardRect);
            cardView.transform.SetParent(_tableController.Table.transform);
            seq.AppendCallback(() =>
            {
                cardRect.DOSizeDelta(Vector2.zero, _deckSettings.drawAnimTime);
                cardRect.DOAnchorPos(Vector2.zero, _deckSettings.drawAnimTime).OnComplete(() =>
                {
                    cardView.SetVisible(false);
                }
                );
            });
            seq.AppendInterval(_deckSettings.drawAnimTime);
        }
        for (int i = 0; i < visibleCardCount; i++)
        {
            var card = deck.DrawCard();
            CreateCardView(card, out CardView cardView, out RectTransform cardRect);
            cardView.transform.SetParent(_tableController.Table.transform);
            seq.AppendCallback(() =>
            {
                cardRect.DOSizeDelta(Vector2.zero, _deckSettings.drawAnimTime);
                cardRect.DOAnchorPos(Vector2.zero, _deckSettings.drawAnimTime).OnComplete(() =>
                {
                    cardView.SetVisible(true);
                }
                );
            });
            seq.AppendInterval(_deckSettings.drawAnimTime);
        }
    }

    private void OnPlayerDrawnCards(IPlayer player, List<ICard> cardsDrawn)
    {
        var playerController = _playerControllers[player];


        foreach (var card in cardsDrawn)
        {
            CardView cardView;
            RectTransform cardRect;
            CreateCardView(card, out cardView, out cardRect);
            playerController.AddCardToHand(cardView);

            cardRect.DOSizeDelta(Vector2.zero, _deckSettings.drawAnimTime);
            cardRect.DOAnchorPos(Vector2.zero, _deckSettings.drawAnimTime).OnComplete(() =>
            {
                cardView.SetVisible(_currentPlayer == player);
            }
            );
        }
    }

    private void CreateCardView(ICard card, out CardView cardView, out RectTransform cardRect)
    {
        cardView = _cardPool.Spawn();
        cardView.transform.SetParent(_deckController.DeckTransform);
        cardRect = cardView.GetComponent<RectTransform>();
        cardRect.offsetMax = Vector2.zero;
        cardRect.offsetMin = Vector2.zero;
        cardRect.sizeDelta = Vector2.zero;

        cardView.SetCard(card);
        cardView.SetVisible(false);
    }
}
