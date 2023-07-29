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
    private ITurnManager _turnManager;

    [Inject]
    public void Construct(RoomManager roomManager, BotController.Factory botFactory, IPlayer player, PlayerControllerBase playerController, CardView.CardPool cardPool, DeckController deckController, Deck.Settings deckSettings, TableController tableController, ITurnManager turnManager)
    {
        _roomManager = roomManager;
        _botFactory = botFactory;
        _cardPool = cardPool;
        _deckController = deckController;
        _currentPlayer = player;
        _deckSettings = deckSettings;
        _tableController = tableController;
        _turnManager = turnManager;

        _roomManager.RoomReady += OnRoomReady;

        _playerControllers.Add(player, playerController);
    }

    private void OnRoomReady(Room room)
    {
        _turnManager.ClearPlayers();
        _turnManager.AddPlayers(room.Players);
        _container.gameObject.SetActive(true);

        _tableController.Initialize(_deckController.Deck);

        //create bots here
        for (int i = 1; i < room.Players.Count; i++)
        {
            var bot = _botFactory.Create();
            bot.SetPlayer(room.Players[i]);
            bot.transform.SetParent(_botParent);
            _playerControllers.Add(room.Players[i], bot);
        }

        //Maybe turn starts here?
        _turnManager.StartNewTurn();
    }
}
