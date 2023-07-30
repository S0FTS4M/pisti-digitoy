using System;
using System.Collections.Generic;
using DG.Tweening;

public class TurnManager : ITurnManager
{
    private readonly List<PlayerBase> _players;
    private TableController _tableController;
    private Deck.Settings _deckSettings;
    private GameRuleManager _gameRuleManager;
    private GameController _gameController;
    private int _currentPlayerIndex;

    public delegate void TurnManagerEventHandler(int playerIndex);
    public event TurnManagerEventHandler TurnStarted;
    public event TurnManagerEventHandler TurnEnded;

    public TurnManager(TableController tableController, Deck.Settings deckSettings, GameRuleManager gameRuleManager, GameController gameController)
    {
        _tableController = tableController;
        _deckSettings = deckSettings;
        _gameRuleManager = gameRuleManager;
        _gameController = gameController;

        _currentPlayerIndex = 0;
        _players = new List<PlayerBase>();
    }

    public int CurrentPlayerIndex => _currentPlayerIndex;

    public PlayerBase CurrentPlayer => _players[_currentPlayerIndex];

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
        seq.AppendCallback(() => CurrentPlayer.TakeTurn());
    }

    public void AddPlayer(PlayerBase player)
    {
        _players.Add(player);
        player.PlayerEndTurn += OnPlayerEndTurn;
    }

    private void OnPlayerEndTurn(PlayerBase player)
    {
        var seq = DOTween.Sequence();
        if (CheckAllPlayersOutOfCards())
        {
            if (player.Deck.CardCount >= _players.Count * _deckSettings.initialDrawCount)
            {
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
            }
            else
            {
                _gameRuleManager.CalculateFinalScores();
                _gameController.GameOver();

            }
        }
        seq.AppendCallback(() => SwitchPlayer());
    }

    private bool CheckAllPlayersOutOfCards()
    {
        var allOutOfCard = true;
        foreach (var player in _players)
        {
            if (player.Hand.Count == 0)
            {
                allOutOfCard &= true;
            }
            else
            {
                allOutOfCard = false;
            }
        }

        return allOutOfCard;
    }

    public void AddPlayers(IEnumerable<PlayerBase> players)
    {
        _players.AddRange(players);
        foreach (var player in players)
        {
            player.PlayerEndTurn += OnPlayerEndTurn;
        }
    }

    public void ClearPlayers()
    {
        foreach (var player in _players)
        {
            player.PlayerEndTurn -= OnPlayerEndTurn;
        }
        _players.Clear();
    }

    public void SwitchPlayer()
    {
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        CurrentPlayer.TakeTurn();
    }
}
