public interface IDeck
{
    event CardDrawnEventHandler CardDrawn;
    void CreateStandardDeck();
    ICard DrawCard();
    void Shuffle();
}

public delegate void CardDrawnEventHandler(ICard card);