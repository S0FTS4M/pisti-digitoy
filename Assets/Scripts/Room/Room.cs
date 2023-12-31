using System.Collections.Generic;

public class Room
{
    private RoomConfig _roomConfig;

    public string Name { get; }
    public int MinBet { get; }
    public int MaxBet { get; }
    public int Bet { get; set; }

    public int CurrentBetAmount { get; set; }

    public List<PlayerBase> Players { get; }
    public RoomConfig Config  => _roomConfig; 

    public Room(RoomConfig roomConfig)
    {
        _roomConfig = roomConfig;
        Name = roomConfig.Name;
        MinBet = roomConfig.MinBet;
        MaxBet = roomConfig.MaxBet;
        Players = new List<PlayerBase>();
    }

    public void JoinRoom(PlayerBase player)
    {
        Players.Add(player);
        player.SetCurrentRoom(this);
    }

    public void AddBot(Bot bot)
    {
        Players.Add(bot);
        bot.SetCurrentRoom(this);
    }

    [System.Serializable]
    public class RoomConfig
    {
        public string Name;
        public int MinBet;
        public int MaxBet;
    }

}
