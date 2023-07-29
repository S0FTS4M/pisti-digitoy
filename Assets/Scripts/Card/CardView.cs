using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CardView : MonoBehaviour
{
    [SerializeField] private Image cardImage;

    private ICard _card;
    private Card.Settings _settings;

    public ICard Card => _card;

    [Inject]
    private void Construct(Card.Settings settings)
    {
        _settings = settings;
    }

    public void SetCard(ICard card)
    {
        _card = card;
        if(card.IsVisible)
            cardImage.sprite = card.Sprite;
        else
            cardImage.sprite = _settings.cardBacks;
    }

    public void SetVisible(bool visible)
    {
        _card.IsVisible = visible;
        
        if(visible)
            cardImage.sprite = _card.Sprite;
        else
            cardImage.sprite = _settings.cardBacks;
    }

    [System.Serializable]
    public class Settings
    {
        public GameObject cardPrefab;
    }

    public class CardPool : MemoryPool<CardView>
    {

    }
}

