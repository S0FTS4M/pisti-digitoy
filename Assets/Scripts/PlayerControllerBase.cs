using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControllerBase : MonoBehaviour
{
    private int _currentSlotIndex = 0;

    public List<Transform> HandSlots { get; } = new List<Transform>();
    
    public virtual Transform AddCardToHand(CardView card)
    {
        var currentSlot = HandSlots[_currentSlotIndex++];
        card.transform.SetParent(currentSlot);
        var cardRect = card.GetComponent<RectTransform>();

        return currentSlot;
    }
}
