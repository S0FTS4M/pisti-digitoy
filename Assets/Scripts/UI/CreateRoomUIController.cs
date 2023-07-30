using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CreateRoomUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject container;

    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private Toggle twoPlayersToggle;

    [SerializeField]
    private Toggle fourPlayersToggle;

    [SerializeField]
    private Slider betSlider;

    [SerializeField]
    private TextMeshProUGUI betText;

    [SerializeField]
    private TextMeshProUGUI minBetText;

    [SerializeField]
    private TextMeshProUGUI maxBetText;

    [SerializeField]
    private Button createButton;

    private int playerCount = 2;

    private int bet;


    private EntranceUIController _entranceUIController;
    private RoomManager _roomManager;
    private PlayerBase _player;

    [Inject]
    private void Construct(EntranceUIController entranceUIController, RoomManager roomManager, PlayerBase playerBase)
    {
        _entranceUIController = entranceUIController;
        _roomManager = roomManager;
        _player = playerBase;
        closeButton.onClick.AddListener(Close);

        twoPlayersToggle.onValueChanged.AddListener(OnTwoPlayersToggleValueChanged);
        fourPlayersToggle.onValueChanged.AddListener(OnFourPlayersToggleValueChanged);
        betSlider.onValueChanged.AddListener(OnBetSliderValueChanged);
    }

    private void OnBetSliderValueChanged(float value)
    {
        bet = (int)value;
        betText.text = value.ToString();
    }

    private void OnFourPlayersToggleValueChanged(bool value)
    {
        if (value)
        {
            playerCount = 4;
        }
    }

    private void OnTwoPlayersToggleValueChanged(bool value)
    {
        if (value)
        {
            playerCount = 2;
        }
    }

    public void Show(Room.RoomConfig roomConfig)
    {
        container.SetActive(true);
        container.transform.DOScale(Vector3.one * 1.1f, .3f).SetEase(Ease.OutBack);

        betSlider.minValue = roomConfig.MinBet;
        betSlider.maxValue = roomConfig.MaxBet;
        betSlider.value = roomConfig.MinBet;
        minBetText.text = roomConfig.MinBet.ToString();
        maxBetText.text = roomConfig.MaxBet.ToString();
        betText.text = roomConfig.MinBet.ToString();
        createButton.onClick.RemoveAllListeners();
        createButton.onClick.AddListener(() =>
        {
            _roomManager.CreateRoom(playerCount, bet, roomConfig);
            Hide();
        });
    }

    public void Hide()
    {
        container.SetActive(false);
    }

    public void Close()
    {
        Hide();
        _entranceUIController.Show();
    }

}
