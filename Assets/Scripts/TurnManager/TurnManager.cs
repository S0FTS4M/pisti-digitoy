using System.Collections.Generic;

public class TurnManager : ITurnManager
{
    private readonly List<IPlayer> _players;
    private int _currentPlayerIndex;

    public delegate void TurnManagerEventHandler(int playerIndex);
    public event TurnManagerEventHandler TurnStarted;
    public event TurnManagerEventHandler TurnEnded;

    public TurnManager()
    {
        _currentPlayerIndex = 0;
        _players = new List<IPlayer>();
    }

    public int CurrentPlayerIndex => _currentPlayerIndex;

    public IPlayer CurrentPlayer => _players[_currentPlayerIndex];

    public void StartNewTurn()
    {
        //TODO: draw 4 cards and place it to the table and show the card at the top

        //TODO: first player takes the turn
    }

    public void AddPlayer(IPlayer player)
    {
        _players.Add(player);
    }

    public void ClearPlayers()
    {
        _players.Clear();
    }

    public void SwitchPlayer()
    {
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        CurrentPlayer.TakeTurn();
    }
}
