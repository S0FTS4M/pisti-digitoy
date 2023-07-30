using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class BotController : PlayerControllerBase
{
    [SerializeField]
    private TextMeshProUGUI _playerName;


    public override void SetPlayer(PlayerBase player)
    {
        base.SetPlayer(player);
        _playerName.text = player.Name;
        _scoreText.text = player.Score.ToString();
        _currencyText.text = player.Currency.Amount.ToString();
    }

    protected override void OnPlayerTurn(PlayerBase player)
    {
        base.OnPlayerTurn(player);

        var topCard = _tableController.GetTopCard();
        if (topCard != null)
        {

            for (int i = 0; i < Player.Hand.Count; i++)
            {
                //NOTE: we can create a Rank enum and put Jack = 11, Queen = 12, King = 13, Ace = 1 but for now we will just check for the value
                if (topCard.Rank == Player.Hand[i].Rank || Player.Hand[i].Rank == 11)
                {
                    var card = Player.Hand[i];
                    PlayCard(card, () => player.PlayCard(card));
                    return;
                }
            }
        }

        var randomCard = Player.Hand[UnityEngine.Random.Range(0, Player.Hand.Count)];
        PlayCard(randomCard, () => player.PlayCard(randomCard));
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
