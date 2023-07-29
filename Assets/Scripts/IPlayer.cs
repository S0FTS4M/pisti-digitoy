using System.Collections.Generic;
using SOFTSAM.Models.CurrencyManagement;

public interface IPlayer
{
    event PlayerCardDrawHandler PlayerDrawnCards;
    Room CurrentRoom { get; set;}
    ICurrencyBase Currency { get; }
    IDeck Deck { get; set; }
    int WinCount { get; }
    int LoseCount { get; }
    int Score { get; }
    List<ICard> DrawCardsFromDeck(int numCardsToDraw = 1);
    List<ICard> GetHand();
    void RemoveCardFromHand(ICard card);
}

public delegate void PlayerCardDrawHandler(IPlayer player, List<ICard> cardsDrawn);