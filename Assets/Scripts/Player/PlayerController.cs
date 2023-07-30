using UnityEngine;
using Zenject;

public class PlayerController : PlayerControllerBase
{
    [Inject]
    private void Construct(PlayerBase player)
    {
        SetPlayer(player);
    }
}
