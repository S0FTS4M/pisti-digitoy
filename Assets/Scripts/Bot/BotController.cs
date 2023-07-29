using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class BotController : PlayerControllerBase
{
    [SerializeField]
    private TextMeshProUGUI _playerName;

    public override void SetPlayer(IPlayer player)
    {
        base.SetPlayer(player);
        _playerName.text = "Bot";
        _scoreText.text = player.Score.ToString();
        _currencyText.text = player.Currency.Amount.ToString();
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
