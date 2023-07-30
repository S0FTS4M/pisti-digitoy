using System;
using System.Collections.Generic;
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

    private Dictionary<PlayerBase, PlayerControllerBase> _playerControllers = new Dictionary<PlayerBase, PlayerControllerBase>();

    private CardView.CardPool _cardPool;

    private DeckController _deckController;

    private PlayerBase _currentPlayer;

    private Deck.Settings _deckSettings;

    private TableController _tableController;
    private ITurnManager _turnManager;

    public event Action OnGameOver;

    [Inject]
    public void Construct(RoomManager roomManager, BotController.Factory botFactory, PlayerBase player, PlayerControllerBase playerController, CardView.CardPool cardPool, DeckController deckController, Deck.Settings deckSettings, TableController tableController, ITurnManager turnManager)
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
        _roomManager.RoomLeft += OnRoomLeft;

        _playerControllers.Add(player, playerController);
    }

    private void OnRoomLeft(Room room)
    {
        foreach(var player in room.Players)
        {
            if(player == _currentPlayer)
                continue;

            var playerController = GetPlayerController(player);
            playerController.ClearAllCards();
            _playerControllers.Remove(player);
            Destroy(playerController.gameObject);
        }
        var currentPlayerController = GetPlayerController(_currentPlayer);
        currentPlayerController.ClearAllCards();
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

    public PlayerControllerBase GetPlayerController(PlayerBase player)
    {
        if(_playerControllers.ContainsKey(player))
        {
            return _playerControllers[player];
        }
        
        return null;
    }

    public void Hide()
    {
        _container.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
    }
}

