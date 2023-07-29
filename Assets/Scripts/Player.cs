
using System.Collections.Generic;
using SOFTSAM.Models.CurrencyManagement;

public class Player : IPlayer
{
    private List<ICard> hand;

    private ICurrencyBase _currency;

    public event PlayerCardDrawHandler PlayerDrawnCards;

    public Player(ICurrencyBase currency)
    {
        _currency = currency;
        hand = new List<ICard>();
    }

    public Room CurrentRoom { get; set; }
    
    public ICurrencyBase Currency => _currency;

    public IDeck Deck { get; set; }

    public int Score { get; private set; }

    public int WinCount { get; private set; }

    public int LoseCount { get; private set; }

    public void DrawInitialCards(int numCardsToDraw)
    {
        for (int i = 0; i < numCardsToDraw; i++)
        {
            DrawCardFromDeck();
        }
    }

    public List<ICard> DrawCardsFromDeck(int numCardsToDraw)
    {
        var drawList = new List<ICard>();
        for (int i = 0; i < numCardsToDraw; i++)
        {
            var card = DrawCardFromDeck();
            drawList.Add(card);
        }
        PlayerDrawnCards?.Invoke(this, drawList);
        return drawList;
    }

    private ICard DrawCardFromDeck()
    {
        ICard drawnCard = Deck.DrawCard();
        if (drawnCard != null)
        {
            AddCardToHand(drawnCard);
        }

        return drawnCard;
    }

    private void AddCardToHand(ICard card)
    {
        hand.Add(card);
    }

    public void RemoveCardFromHand(ICard card)
    {
        hand.Remove(card);
    }

    public List<ICard> GetHand()
    {
        return new List<ICard>(hand);
    }
}
