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

    private Room room;
    private Deck.Settings _deckSettings;
    private GameController _gameController;

    public IPlayer Player => _player;

    public List<Transform> HandSlots { get; } = new List<Transform>();
    
    [Inject]
    private void Construct(Deck.Settings deckSettings, GameController gameController)
    {
        _deckSettings = deckSettings;
        _gameController = gameController;
    }

    public virtual void SetPlayer(IPlayer player)
    {
        _player = player;

        foreach (Transform child in handSlotParent)
        {
            HandSlots.Add(child);
        }

        _player.PlayerDrawnCards += OnDrawCard;
    }

    private void OnDrawCard(IPlayer player, List<ICard> cardsDrawn)
    {
        foreach (var card in cardsDrawn)
        {
            CardView cardView;
            RectTransform cardRect;
            _gameController.CreateCardView(card, out cardView, out cardRect);
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

    }

    public virtual void DrawCard(int count)
    {
        var seq = DOTween.Sequence();

        for (int cardCount = 0; cardCount < 4; cardCount++)
        {
            for (int i = 0; i < room.Players.Count; i++)
            {
                var player = room.Players[i];
                seq.AppendCallback(() => player.DrawCardsFromDeck(1));
                seq.AppendInterval(_deckSettings.drawAnimDelay);
            }
        }

    }
}
