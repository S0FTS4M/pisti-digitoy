using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class TableController : MonoBehaviour
{
    [SerializeField] private GameObject _table;

    private Deck _deck;
    private Deck.Settings _deckSettings;
    private DeckController _deckController;

    public GameObject Table => _table;

    public List<ICard> Cards { get; private set; }

    [Inject]
    private void Construct(DeckController deckController, Deck.Settings deckSettings)
    {
        _deckSettings = deckSettings;
        _deckController = deckController;
    }

    public void Initialize(Deck deck)
    {
        _deck = deck;

        Cards = new List<ICard>();
    }
    public void PutCard(CardView cardView, bool isVisible, Action onComplete)
    {
        var seq = DOTween.Sequence();

        Cards.Add(cardView.Card);

        var cardRect = cardView.GetComponent<RectTransform>();
        cardView.transform.SetParent(Table.transform);

        cardRect.DOSizeDelta(Vector2.zero, _deckSettings.drawAnimTime);
        cardRect.DOAnchorPos(Vector2.zero, _deckSettings.drawAnimTime).OnComplete(() =>
        {
            cardView.SetVisible(isVisible);
            onComplete?.Invoke();
        }
        );
    }

    public void PutCard(ICard card, bool isVisible, Action onComplete)
    {
        _deckController.CreateCardView(card, out CardView cardView, out RectTransform cardRect);
        PutCard(cardView, isVisible, onComplete);
    }

    public void PutStartingCards()
    {
        var seq = DOTween.Sequence();
        for (int i = 0; i < _deckSettings.initialDrawCount; i++)
        {
            var card = _deck.DrawCard();

            var isLastCard = i == _deckSettings.initialDrawCount - 1;
            seq.AppendCallback(() =>
            {
                PutCard(card, isLastCard, null);
            });
            seq.AppendInterval(_deckSettings.drawAnimTime);
        }
    }

    public ICard GetTopCard()
    {
        if(Cards.Count == 0)
            return null;
            
        return Cards[Cards.Count - 1];
    }
}
