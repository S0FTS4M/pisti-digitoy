using System.Collections.Generic;
using DG.Tweening;

public class TurnManager : ITurnManager
{
    private readonly List<IPlayer> _players;
    private TableController _tableController;
    private int _currentPlayerIndex;

    public delegate void TurnManagerEventHandler(int playerIndex);
    public event TurnManagerEventHandler TurnStarted;
    public event TurnManagerEventHandler TurnEnded;

    public TurnManager(TableController tableController)
    {
        _tableController = tableController;
        _currentPlayerIndex = 0;
        _players = new List<IPlayer>();
    }

    public int CurrentPlayerIndex => _currentPlayerIndex;

    public IPlayer CurrentPlayer => _players[_currentPlayerIndex];

    public void StartNewTurn()
    {
        var seq = DOTween.Sequence();

        seq.AppendCallback(() => _tableController.PutStartingCards());
        seq.AppendInterval(1f);

        for (int i = 0; i < _players.Count; i++)
        {
            int index = i;
            seq.AppendCallback(() =>
            {
                _players[index].RequestCardDraw(4);
            }
            );
            seq.AppendInterval(1f);
        }
        CurrentPlayer.TakeTurn();
    }

    public void AddPlayer(IPlayer player)
    {
        _players.Add(player);
    }

    public void AddPlayers(IEnumerable<IPlayer> players)
    {
        _players.AddRange(players);
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
