using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "PlayingCards/Card", order = 1)]
public class CardInfo : ScriptableObject
{
    public int rank;
    public string suit;
    public Sprite cardImage;
}
