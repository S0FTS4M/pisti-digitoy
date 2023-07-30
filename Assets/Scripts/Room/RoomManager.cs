using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RoomManager
{
    private PlayerBase player;

    private Room _currentRoom;

    private Bot.Factory _botFactory;

    public Room CurrentRoom  => _currentRoom; 

    public delegate void RoomHandler(Room room);

    public event RoomHandler RoomCreated;
    public event RoomHandler RoomJoined;
    public event RoomHandler RoomReady;
    public event RoomHandler RoomLeft;
    public event Action<Room.RoomConfig> RoomCreationFailed;

    [Inject]
    private void Construct(PlayerBase player, Bot.Factory botFactory)
    {
        _botFactory = botFactory;
        this.player = player;
    }

    public bool CreateRoom(int playerCount, int playerBet, Room.RoomConfig roomConfig)
    {
        if (player.Currency.CanAfford(playerBet) && player.Currency.CanAfford(roomConfig.MinBet))
        {
            _currentRoom = new Room(roomConfig);

            RoomCreated?.Invoke(_currentRoom);

            _currentRoom.Bet = (int)playerBet;

            _currentRoom.JoinRoom(player);
            _currentRoom.CurrentBetAmount += _currentRoom.Bet;
            for (int i = 0; i < playerCount - 1; i++)
            {
                var bot = _botFactory.Create();
                _currentRoom.AddBot(bot);
                _currentRoom.CurrentBetAmount += _currentRoom.Bet;
            }
            RoomJoined?.Invoke(_currentRoom);
            RoomReady?.Invoke(_currentRoom);
            return true;
        }
        else
        {
            RoomCreationFailed?.Invoke(roomConfig);
            return false;
        }
    }

    internal void LeaveRoom()
    {
        if(CurrentRoom == null)
            return;
        RoomLeft?.Invoke(_currentRoom);
        _currentRoom = null;
    }

    [System.Serializable]
    public class Settings
    {
        public GameObject RoomPrefab;
        
        public List<Room.RoomConfig> RoomConfigs;

        public int DefaultPlayerCount;
    }
}
