using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class BotController : PlayerControllerBase
{
    [SerializeField]
    private TextMeshProUGUI _playerName;

    [SerializeField]
    private Transform hand;

    [SerializeField]
    private TextMeshProUGUI _playerScoreText;

    [SerializeField]
    private TextMeshProUGUI _playerMoneyText;

    public void SetPlayer(IPlayer player)
    {
        _playerName.text = "Bot";
        _playerScoreText.text = player.Score.ToString();
        _playerMoneyText.text = player.Currency.Amount.ToString();

        foreach(Transform handSlot in hand)
        {
            HandSlots.Add(handSlot);
        }
    }

    public class Factory : PlaceholderFactory<BotController>
    {
    }

    [System.Serializable]
    public class Settings
    {
        public GameObject BotPrefab;
    }
}
