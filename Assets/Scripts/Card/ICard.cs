using UnityEngine;

public interface ICard
{
    int Rank { get; }
    string Suit { get; }
    Sprite Sprite { get; }
    bool IsVisible { get; set; }
}
