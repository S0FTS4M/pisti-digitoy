using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject container;

    [SerializeField]
    private TextMeshProUGUI winCountText;

    [SerializeField]
    private TextMeshProUGUI loseCountText;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private TextMeshProUGUI playerNameText;

    public void Show(PlayerBase player)
    {
        container.SetActive(true);
        winCountText.text = player.WinCount.ToString();
        loseCountText.text = player.LoseCount.ToString();
        playerNameText.text = player.Name;

        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(() =>Hide());
    }

    public void Hide()
    {
        container.SetActive(false);
    }
}
    

