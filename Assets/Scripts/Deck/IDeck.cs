public interface IDeck
{
    event CardDrawnEventHandler CardDrawn;
    void CreateStandardDeck();
    ICard DrawCard();
    void Shuffle();

    int CardCount { get; }
}

public delegate void CardDrawnEventHandler(ICard card);