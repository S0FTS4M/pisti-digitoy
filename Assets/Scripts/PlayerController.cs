using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class PlayerController : PlayerControllerBase
{
    [SerializeField]
    private Transform handSlotParent;

    [SerializeField]
    private TextMeshProUGUI _currencyText;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    private IPlayer _player;

    [Inject]
    private void Construct(IPlayer player)
    {
        _player = player;
        foreach (Transform child in handSlotParent)
        {
            HandSlots.Add(child);
        }
    }
}
