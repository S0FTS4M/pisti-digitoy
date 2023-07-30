using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RoomUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomNameText;

    [SerializeField]
    private TextMeshProUGUI betRangeText;

    [SerializeField]
    private Button playNowButton;

    [SerializeField]
    private Button createTableButton;

    private EntranceUIController _entranceUIController;

    private PlayerBase _player;

    private RoomManager _roomManager;
    private RoomManager.Settings _roomManagerSettings;
    private CreateRoomUIController _createRoomUIController;
    private Room.RoomConfig _roomConfig;

    [Inject]
    private void Construct(EntranceUIController entranceUIController, PlayerBase player, RoomManager roomManager, RoomManager.Settings roomManagerSettings, CreateRoomUIController createRoomUIController)
    {
        _entranceUIController = entranceUIController;
        _player = player;
        _roomManager = roomManager;
        _roomManagerSettings = roomManagerSettings;
        _createRoomUIController = createRoomUIController;

        _roomManager.RoomCreated += OnRoomCreated;
        _roomManager.RoomCreationFailed += OnRoomCreationFailed;
    }

    public void SetRoom(Room.RoomConfig roomConfig)
    {
        _roomConfig = roomConfig;
        roomNameText.text = roomConfig.Name;
        betRangeText.text = $"Bet Range: {roomConfig.MinBet} - {roomConfig.MaxBet}";
        playNowButton.onClick.AddListener(OnPlayButtonClicked);
        createTableButton.onClick.AddListener(() =>
        {
            _createRoomUIController.Show(roomConfig);
            _entranceUIController.Hide();
        });
    }

    private void OnRoomCreated(Room room)
    {
        if (room.Name != _roomConfig.Name)
            return;
        _entranceUIController.Hide();
    }

    private void OnRoomCreationFailed(Room.RoomConfig roomConfig)
    {
        if (roomConfig.Name != _roomConfig.Name)
            return;
        Debug.Log("You can not join this room with your current currency");
    }

    private void OnPlayButtonClicked()
    {
        var maxPlayerCanBet = Mathf.Min((float)_player.Currency.Amount, _roomConfig.MaxBet);
        _roomManager.CreateRoom(_roomManagerSettings.DefaultPlayerCount, maxPlayerCanBet, _roomConfig);
    }

    public class Factory : PlaceholderFactory<RoomUIController>
    {
    }
}
