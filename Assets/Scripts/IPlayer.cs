using System.Collections.Generic;
using SOFTSAM.Models.CurrencyManagement;

public interface IPlayer
{
    event PlayerCardDrawHandler PlayerDrawnCards;
    event CardDrawRequestedHandler PlayerRequestedCardDraw;

    Room CurrentRoom { get; set;}
    ICurrencyBase Currency { get; }
    IDeck Deck { get; set; }
    int WinCount { get; }
    int LoseCount { get; }
    int Score { get; }
    List<ICard> DrawCardsFromDeck(int numCardsToDraw = 1);
    List<ICard> GetHand();
    void RemoveCardFromHand(ICard card);
    void TakeTurn();
    void RequestCardDraw(int count);
}

public delegate void PlayerCardDrawHandler(IPlayer player, List<ICard> cardsDrawn);
public delegate void CardDrawRequestedHandler(int count);