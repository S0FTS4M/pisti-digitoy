
using System.Collections.Generic;
using Zenject;

public class Deck : IDeck
{
    private IList<ICard> cards;

    public int CardCount => cards.Count;

    public event CardDrawnEventHandler CardDrawn;

    public Deck(IList<ICard> cards)
    {
        this.cards = cards;
    }

    public void CreateStandardDeck()
    {
        // Create a standard deck with 52 cards (13 ranks and 4 suits)
        // Add cards to the 'cards' list
    }

    public void Shuffle()
    {
        cards.Shuffle();
    }

    public ICard DrawCard()
    {
        if(cards.Count > 0 )
        {
            var card = cards[0];
            cards.RemoveAt(0);
            CardDrawn?.Invoke(card);
            return card;
        }
        return null;
    }

    public class Factory : PlaceholderFactory<Deck>
    {

    }

    [System.Serializable]
    public class Settings
    {
        public List<CardInfo> cardInfos;
        public float drawAnimTime;
        public float drawAnimDelay;
        public int initialDrawCount;
    }
}