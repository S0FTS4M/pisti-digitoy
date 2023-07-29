using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        var card = GetTileUnderTouch(eventData);
        if(card == null)
            return;
        Debug.Log(card.Card.Suit + " " + card.Card.Rank);
    }


    private CardView GetTileUnderTouch(PointerEventData eventData)
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
