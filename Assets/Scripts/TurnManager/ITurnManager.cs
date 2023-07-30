using System.Collections.Generic;

public interface ITurnManager
{
    int CurrentPlayerIndex { get; }
    PlayerBase CurrentPlayer { get; }

    event TurnManager.TurnManagerEventHandler TurnStarted;
    event TurnManager.TurnManagerEventHandler TurnEnded;

    void AddPlayer(PlayerBase player);
    void AddPlayers(IEnumerable<PlayerBase> players);
    void ClearPlayers();
    void StartNewTurn();
    void SwitchPlayer();
}
