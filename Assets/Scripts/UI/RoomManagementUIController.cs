using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RoomManagementUIController : MonoBehaviour
{
    [SerializeField]
    private Transform _roomUIParent;

    private RoomUIController.Factory _roomUIControllerFactory;

    [Inject]
    private void Construct(RoomManager.Settings roomManagerSettings, RoomUIController.Factory roomUIControllerFactory, PlayerBase pla)
    {
        _roomUIControllerFactory = roomUIControllerFactory;

        for (int i = 0; i < roomManagerSettings.RoomConfigs.Count; i++)
        {
            var roomUIController = _roomUIControllerFactory.Create();
            roomUIController.SetRoom(roomManagerSettings.RoomConfigs[i]);
            roomUIController.transform.SetParent(_roomUIParent);
        }
    }
}
