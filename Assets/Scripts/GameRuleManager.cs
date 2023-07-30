using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameRuleManager
{
    private TableController _tableController;
    private RoomManager _roomManager;
    private GameController _gameController;

    private Room _currentRoom;

    public GameRuleManager(TableController tableController, RoomManager roomManager, GameController gameController)
    {
        this._tableController = tableController;
        this._roomManager = roomManager;
        this._gameController = gameController;


        roomManager.RoomJoined += OnRoomJoined;
        roomManager.RoomLeft += OnRoomLeft;
    }


    private void OnRoomJoined(Room room)
    {
        _currentRoom = room;
        foreach (var player in room.Players)
        {
            player.PlayerPlayedCard += OnPlayerPlayedCard;
        }
    }
    
    private void OnRoomLeft(Room room)
    {
        _currentRoom = null;
        foreach (var player in room.Players)
        {
            player.PlayerPlayedCard -= OnPlayerPlayedCard;
        }
    }

    private void OnPlayerPlayedCard(PlayerBase player, ICard card)
    {
        var seq = DOTween.Sequence();
        var playerController = _gameController.GetPlayerController(player);
        if (_tableController.Cards.Count > 2)
        {
            var topCard = _tableController.Cards[_tableController.Cards.Count - 1];
            var secondTopCard = _tableController.Cards[_tableController.Cards.Count - 2];
            if (topCard.Rank == secondTopCard.Rank || topCard.Rank == 11)
            {
                //player wons the cards
                playerController.WonTheCardsOnTheTable(seq);
            }
        }

        if (_tableController.Cards.Count == 2)
        {
            var topCard = _tableController.Cards[_tableController.Cards.Count - 1];
            var secondTopCard = _tableController.Cards[_tableController.Cards.Count - 2];
            if (topCard.Rank == secondTopCard.Rank)
            {
                //player wons the cards
                playerController.Phisti(seq);
            }
        }

        seq.AppendCallback(player.EndTurn);

    }
    public int GetScoreForCard(ICard card)
    {
        if (card.Rank == 10 && card.Suit == "Diamonds")
        {
            return 3;
        }
        else if (card.Rank == 2 && card.Suit == "Clubs")
        {
            return 2;
        }
        else if (card.Rank == 1 || card.Rank == 11)
        {
            return 1;
        }

        return 0;
    }

    public int GetScoreForPishti()
    {
        var cardOnTable = _tableController.Cards;
        var firstCard = cardOnTable[0];
        if (firstCard.Rank == 11)
        {
            return 20;
        }
        else
        {
            return 10;
        }
    }
}

