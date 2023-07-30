using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InputManager : MonoBehaviour, IPointerDownHandler
{
    private PlayerControllerBase _playerController;

    [Inject]
    private void Construct(PlayerControllerBase playerController)
    {
        _playerController = playerController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_playerController.HasTurn == false)
            return;

        var cardView = GetCardUnderTouch(eventData);
        if (cardView == null)
            return;

        //CHECK: this is bugged
        if (_playerController.Player.HasCardInHand(cardView.Card) == false)
            return;

        _playerController.PlayCard(cardView, ()=> _playerController.Player.PlayCard(cardView.Card));
    }

    private CardView GetCardUnderTouch(PointerEventData eventData)
    {
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);
        for (int i = 0; i < result.Count; i++)
        {
            var tile = result[i].gameObject.GetComponentInParent<CardView>();
            if (tile != null)
            {
                return tile;
            }
        }

        return null;
    }
}
