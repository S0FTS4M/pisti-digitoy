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

    [SerializeField]
    protected Transform _wonCardsPileParent;

    private int _currentSlotIndex = 0;

    private PlayerBase _player;

    private Deck.Settings _deckSettings;
    private DeckController _deckController;
    protected TableController _tableController;
    private GameRuleManager _gameRuleManager;

    public bool HasTurn { get; set; }

    public PlayerBase Player => _player;

    public List<Transform> HandSlots { get; } = new List<Transform>();

    [Inject]
    private void Construct(Deck.Settings deckSettings, DeckController deckController, TableController tableController, GameRuleManager gameRuleManager)
    {
        _deckSettings = deckSettings;
        _deckController = deckController;
        _tableController = tableController;
        _gameRuleManager = gameRuleManager;
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
        _player.PlayerScored += OnPlayerScored;
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

    public void ClearAllCards()
    {
        foreach (var handSlot in HandSlots)
        {
            foreach (Transform child in handSlot)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (Transform child in _wonCardsPileParent)
        {
            Destroy(child.gameObject);
        }
        Player.ClearHand();
        HasTurn = false;
    }

    protected virtual void OnPlayerTurn(PlayerBase player)
    {
        HasTurn = true;
    }

    private void OnPlayerEndTurn(PlayerBase player)
    {
        HasTurn = false;
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

    public virtual void WonTheCardsOnTheTable(Sequence sequence)
    {
        var cardOnTable = _tableController.Cards;
        foreach (var card in cardOnTable)
        {
            Player.AddScore(_gameRuleManager.GetScoreForCard(card));
        }

        AnimateCards(sequence);

    }

    public virtual void Phisti(Sequence sequence)
    {
        Player.AddScore(_gameRuleManager.GetScoreForPishti());
        //Notify UI for pishti

        AnimateCards(sequence);
    }

    private void AnimateCards(Sequence seq)
    {
        for(int i = _tableController.Table.transform.childCount - 1; i >= 0; i--)
        {
            var cardViewTransform = _tableController.Table.transform.GetChild(i);
            var cardView = cardViewTransform.GetComponent<CardView>();
            cardViewTransform.SetParent(_wonCardsPileParent);
            var rectTansform = cardViewTransform.GetComponent<RectTransform>();
            seq.AppendCallback(() =>
            {
                rectTansform.DOAnchorPos(Vector2.zero, .1f);
                rectTansform.DOSizeDelta(Vector2.zero, .1f);
            });
            seq.AppendInterval(.1f);
            seq.AppendCallback(() => cardView.SetVisible(false));
            Player.AddWonCard(cardView.Card);
        }

        _tableController.Cards.Clear();
    }

    private void OnPlayerScored(int score)
    {
        _scoreText.SetText(score.ToString());
    }

}
