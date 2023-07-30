using System;
using UnityEngine;
using Zenject;

public class DeckController : MonoBehaviour
{
    [SerializeField]
    private Transform deckTransform;

    private Deck.Factory _deckFactory;
    private CardView.CardPool _cardPool;
    private Deck _deck;

    public Transform DeckTransform => deckTransform;

    public Deck Deck { get => _deck; set => _deck = value; }

    [Inject]
    private void Construct(RoomManager roomManager, Deck.Factory deckFactory, CardView.CardPool cardPool)
    {
        this._deckFactory = deckFactory;
        this._cardPool = cardPool;
        roomManager.RoomCreated += OnRoomCreated;
        roomManager.RoomJoined += OnPlayersJoined;
    }

    public void CreateCardView(ICard card, out CardView cardView, out RectTransform cardRect)
    {
        cardView = _cardPool.Spawn();
        cardView.transform.SetParent(DeckTransform);
        cardRect = cardView.GetComponent<RectTransform>();
        cardRect.offsetMax = Vector2.zero;
        cardRect.offsetMin = Vector2.zero;
        cardRect.sizeDelta = Vector2.zero;

        cardView.SetCard(card);
        cardView.SetVisible(false);
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
        if(_deck != null)
            _deck.CardDrawn -= OnCardDrawn;
        _deck = _deckFactory.Create();
        _deck.Shuffle();
        _deck.CardDrawn += OnCardDrawn;
    }

    private void OnCardDrawn(ICard card)
    {
        deckTransform.gameObject.SetActive(_deck.CardCount != 0);
    }
}