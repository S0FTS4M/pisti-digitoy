using System;
using System.Collections.Generic;
using SOFTSAM.Models.CurrencyManagement;

public abstract class PlayerBase
{
    protected List<ICard> hand;

    private List<ICard> wonCards;

    private ICurrencyBase _currency;

    protected string name;

    public event PlayerCardDrawHandler PlayerDrawnCards;
    public event CardDrawRequestedHandler PlayerRequestedCardDraw;
    public event PlayerCardPlayedHandler PlayerPlayedCard;
    public event PlayerTurnHandler PlayerTurn;
    public event PlayerTurnHandler PlayerEndTurn;
    public event ScoreHandler PlayerScored;

    public PlayerBase(ICurrencyBase currency)
    {
        _currency = currency;
        hand = new List<ICard>();
        wonCards = new List<ICard>();
    }

    public Room CurrentRoom { get; set; }

    public ICurrencyBase Currency => _currency;

    public IDeck Deck { get; set; }

    public int Score { get; protected set; }

    public int WinCount { get; set; }

    public int LoseCount { get; set; }

    public List<ICard> Hand => hand;

    public List<ICard> WonCards => wonCards;

    public string Name  => name;

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

    public bool HasCardInHand(ICard card)
    {
        return hand.Contains(card);
    }

    public List<ICard> GetHand()
    {
        return new List<ICard>(hand);
    }

    public virtual void TakeTurn()
    {
        //Activate card interaction
        PlayerTurn?.Invoke(this);
    }

    public virtual void PlayCard(ICard card)
    {
        PlayerPlayedCard?.Invoke(this, card);
    }

    public virtual void AddScore(int score)
    {
        Score += score;
        PlayerScored?.Invoke(Score);
    }   

    public void EndTurn()
    {
        PlayerEndTurn?.Invoke(this);
    }

    public void RequestCardDraw(int count)
    {
        PlayerRequestedCardDraw?.Invoke(count);
    }

    public void AddWonCard(ICard card)
    {
        wonCards.Add(card);
    }

    public void ClearHand()
    {
        hand.Clear();
        wonCards.Clear();
    }
}

public delegate void PlayerCardDrawHandler(PlayerBase player, List<ICard> cardsDrawn);
public delegate void CardDrawRequestedHandler(int count);
public delegate void PlayerTurnHandler(PlayerBase player);
public delegate void PlayerCardPlayedHandler(PlayerBase player, ICard card);
public delegate void ScoreHandler(int score);