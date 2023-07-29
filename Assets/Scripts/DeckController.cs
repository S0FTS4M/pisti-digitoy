using System;
using UnityEngine;
using Zenject;

public class DeckController : MonoBehaviour
{
    [SerializeField]
    private Transform deckTransform;

    private Deck.Factory _deckFactory;

    private Deck _deck;

    public Transform DeckTransform => deckTransform;

    public Deck Deck { get => _deck; set => _deck = value; }

    [Inject]
    private void Construct(RoomManager roomManager, Deck.Factory deckFactory)
    {
        this._deckFactory = deckFactory;
        roomManager.RoomCreated += OnRoomCreated;
        roomManager.RoomJoined += OnPlayersJoined;
    }

    private void OnPlayersJoined(Room room)
    {
        for (int i = 0; i < room.Players.Count; i++)
        {
            room.Players[i].Deck = _deck;
        }
    }

    private void OnRoomCreated(Room room)
    {
        _deck = _deckFactory.Create();

    }
}