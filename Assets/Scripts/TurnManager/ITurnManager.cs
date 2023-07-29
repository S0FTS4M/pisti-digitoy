using System.Collections.Generic;

public interface ITurnManager
{
    int CurrentPlayerIndex { get; }
    IPlayer CurrentPlayer { get; }

    event TurnManager.TurnManagerEventHandler TurnStarted;
    event TurnManager.TurnManagerEventHandler TurnEnded;

    void AddPlayer(IPlayer player);
    void AddPlayers(IEnumerable<IPlayer> players);
    void ClearPlayers();
    void StartNewTurn();
    void SwitchPlayer();
}
