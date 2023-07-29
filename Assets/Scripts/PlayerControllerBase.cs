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

    private IPlayer _player;

    private Deck.Settings _deckSettings;
    private DeckController _deckController;
    private TableController _tableController;

    public IPlayer Player => _player;

    public List<Transform> HandSlots { get; } = new List<Transform>();

    [Inject]
    private void Construct(Deck.Settings deckSettings, DeckController deckController, TableController tableController)
    {
        _deckSettings = deckSettings;
        _deckController = deckController;
        _tableController = tableController;
    }

    public virtual void SetPlayer(IPlayer player)
    {
        _player = player;
        _currentSlotIndex = 0;

        foreach (Transform child in handSlotParent)
        {
            HandSlots.Add(child);
        }
        _player.PlayerRequestedCardDraw += OnDrawCardRequested;
        _player.PlayerDrawnCards += OnDrawCard;
    }


    private void OnDrawCard(IPlayer player, List<ICard> cardsDrawn)
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

    public virtual void PlayCard(CardView card)
    {
        // _tableController.
    }

    private void OnDrawCardRequested(int count)
    {
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
