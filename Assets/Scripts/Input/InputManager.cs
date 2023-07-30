using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InputManager : MonoBehaviour, IPointerDownHandler
{
    private PlayerControllerBase _playerController;
    private PlayerUIController _playerUIController;

    [Inject]
    private void Construct(PlayerControllerBase playerController, PlayerUIController playerUIController)
    {
        _playerController = playerController;
        _playerUIController = playerUIController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var cardView = GetComponentUnderTouch<CardView>(eventData);
        var playerController = GetComponentUnderTouch<PlayerControllerBase>(eventData);
        if (playerController != null && cardView == null)
        {
            _playerUIController.Show(playerController.Player);
        }
        
        if (_playerController.HasTurn == false)
            return;

        if (cardView == null)
            return;

        //CHECK: this is bugged
        if (_playerController.Player.HasCardInHand(cardView.Card) == false)
            return;

        _playerController.PlayCard(cardView, ()=> _playerController.Player.PlayCard(cardView.Card));
    }

    //NOTE: I can move this function into a utility class later on
    private T GetComponentUnderTouch<T>(PointerEventData eventData) where T : Component
    {
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);
        for (int i = 0; i < result.Count; i++)
        {
            var component = result[i].gameObject.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
        }

        return null;
    }
}
