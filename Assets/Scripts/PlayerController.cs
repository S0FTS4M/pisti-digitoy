using UnityEngine;
using Zenject;

public class PlayerController : PlayerControllerBase
{
    [Inject]
    private void Construct(IPlayer player)
    {
        SetPlayer(player);

    }
}
