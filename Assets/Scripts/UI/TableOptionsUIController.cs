using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TableOptionsUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject container;

    [SerializeField]
    private RectTransform containerRect;

    [SerializeField]
    private Button newGameButton;

    [SerializeField]
    private Button backToLobbyButton;

    [Inject]
    private void Construct(GameController gameController, EntranceUIController entranceUIController, RoomManager roomManager)
    {
        backToLobbyButton.onClick.AddListener(() =>
        {
            roomManager.LeaveRoom();
            gameController.Hide();
            entranceUIController.Show();
            Hide();
        }
        );

        newGameButton.onClick.AddListener(() =>
        {
            var currentRoom = roomManager.CurrentRoom;
            if(currentRoom == null)
            {
                return;
            }
            roomManager.LeaveRoom();
            var success = roomManager.CreateRoom(currentRoom.Players.Count, currentRoom.MinBet, currentRoom.Config);
            if (success)
                Hide();
        }
        );
    }

    public void Show()
    {
        container.SetActive(true);
        containerRect.DOAnchorPos(Vector2.zero, 0.5f);
    }

    public void Hide()
    {
        containerRect.DOAnchorPos(new Vector2(655, 0), 0.5f).OnComplete(() =>
        {
            container.SetActive(false);
        });
    }
}
