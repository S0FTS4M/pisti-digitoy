using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EntranceUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject roomContainer;


    public void Show()
    {
        roomContainer.SetActive(true);
    }
    public void Hide()
    {
        roomContainer.SetActive(false);

    }
}
