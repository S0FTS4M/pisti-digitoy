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

    public void CreateRoom(int playerCount, double playerBet, Room.RoomConfig roomConfig)
    {
        if (player.Currency.CanAfford(roomConfig.MinBet))
        {
            _currentRoom = new Room(roomConfig);

            RoomCreated?.Invoke(_currentRoom);

            _currentRoom.JoinRoom(player);
            for (int i = 0; i < playerCount - 1; i++)
            {
                var bot = _botFactory.Create();
                _currentRoom.AddBot(bot);
            }
            RoomJoined?.Invoke(_currentRoom);
            RoomReady?.Invoke(_currentRoom);
        }
        else
        {
            RoomCreationFailed?.Invoke(roomConfig);
        }
    }

    internal void LeaveRoom()
    {
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
