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

    public override void SetPlayer(IPlayer player)
    {
        base.SetPlayer(player);
        _playerName.text = "Bot";
        _playerScoreText.text = player.Score.ToString();
        _playerMoneyText.text = player.Currency.Amount.ToString();
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
