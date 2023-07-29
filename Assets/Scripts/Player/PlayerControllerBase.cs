using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public abstract class PlayerControllerBase : MonoBehaviour
{
    [SerializeField]
    protected Transform handSlotParent;

    [SerializeField]
    protected TextMeshProUGUI _currencyText;

    [SerializeField]
    protected TextMeshProUGUI _scoreText;

    private int _currentSlotIndex = 0;

    private PlayerBase _player;

    private Deck.Settings _deckSettings;
    private DeckController _deckController;
    protected TableController _tableController;

    public bool HasTurn { get; set; }

    public PlayerBase Player => _player;

    public List<Transform> HandSlots { get; } = new List<Transform>();

    [Inject]
    private void Construct(Deck.Settings deckSettings, DeckController deckController, TableController tableController)
    {
        _deckSettings = deckSettings;
        _deckController = deckController;
        _tableController = tableController;
    }

    public virtual void SetPlayer(PlayerBase player)
    {
        _player = player;
        _currentSlotIndex = 0;

        foreach (Transform child in handSlotParent)
        {
            HandSlots.Add(child);
        }
        _player.PlayerTurn += OnPlayerTurn;
        _player.PlayerEndTurn += OnPlayerEndTurn;
        _player.PlayerRequestedCardDraw += OnDrawCardRequested;
        _player.PlayerDrawnCards += OnDrawCard;
    }

    private void OnDrawCard(PlayerBase player, List<ICard> cardsDrawn)
    {
        foreach (var card in cardsDrawn)
        {
            CardView cardView;
            RectTransform cardRect;
            _deckController.CreateCardView(card, out cardView, out cardRect);
            AddCardToHand(cardView);

            cardRect.DOSizeDelta(Vector2.zero, _deckSettings.drawAnimTime);
            cardRect.DOAnchorPos(Vector2.zero, _deckSettings.drawAnimTime).OnComplete(() =>
            {
                cardView.SetVisible(this is PlayerController);
            }
            );
        }
    }

    public virtual Transform AddCardToHand(CardView card)
    {
        var currentSlot = HandSlots[_currentSlotIndex++];
        card.transform.SetParent(currentSlot);
        var cardRect = card.GetComponent<RectTransform>();

        return currentSlot;
    }
    
    public virtual void PlayCard(CardView cardView, Action onComplete)
    {
        _tableController.PutCard(cardView, true, onComplete);
        _player.RemoveCardFromHand(cardView.Card);
    }

    public virtual void PlayCard(ICard card, Action onComplete)
    {
        var cardView = FindCard(card);
        if (cardView == null)
            return;

        PlayCard(cardView, onComplete);
    }

    public void ResetHand()
    {
        _currentSlotIndex = 0;
    }

    public CardView FindCard(ICard card)
    {
        foreach (var handSlot in HandSlots)
        {
            var cardInSlot = handSlot.GetComponentInChildren<CardView>();
            if (cardInSlot != null && cardInSlot.Card == card)
                return handSlot.GetComponentInChildren<CardView>();
        }

        return null;
    }

    protected virtual void OnPlayerTurn(PlayerBase player)
    {
        HasTurn = true;
    }

    private void OnPlayerEndTurn(PlayerBase player)
    {
        HasTurn = false;
        Debug.Log("end turnnnn");
    }

    private void OnDrawCardRequested(int count)
    {
        ResetHand();
        DrawCard(count);
    }

    public virtual void DrawCard(int count)
    {
        var seq = DOTween.Sequence();

        for (int cardCount = 0; cardCount < count; cardCount++)
        {
            seq.AppendCallback(() => _player.DrawCardsFromDeck(1));
            seq.AppendInterval(_deckSettings.drawAnimDelay);
        }

    }
}
