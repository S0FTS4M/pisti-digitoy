
using UnityEngine;
using Zenject;

public class Card : ICard
{
    public int Rank { get; }
    public string Suit { get; }

    public Sprite Sprite { get; }
    
    public bool IsVisible { get; set; }

    public Card(int rank, string suit, Sprite sprite)
    {
        Rank = rank;
        Suit = suit;
        Sprite = sprite;
    }

    [System.Serializable]
    public class Settings
    {
        public Sprite cardBacks;
    }

    public class Factory : PlaceholderFactory<int, string, Sprite, ICard>
    {
        public override ICard Create(int rank, string suit, Sprite sprite) 
        {
            return new Card(rank, suit, sprite);
        }
    }
}