using System.Collections.Generic;
using SOFTSAM.Models.CurrencyManagement;
using Zenject;

public class Bot : IPlayer
{
    public Room CurrentRoom { get; set; }

    public ICurrencyBase Currency { get; private set; }

    public IDeck Deck { get; set; }

    public int Score { get; private set; }

    public int WinCount { get; private set; }

    public int LoseCount { get; private set; }

    public event PlayerCardDrawHandler PlayerDrawnCards;

    private List<ICard> hand;

    public Bot(ICurrencyBase currencyBase)
    {
        Currency = currencyBase;
        Score = 0;
        WinCount = 0;
        LoseCount = 0;
        currencyBase.SetAmount(10000);
        hand = new List<ICard>();
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

    public class Factory : PlaceholderFactory<Bot>
    {

    }

}
