using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayUIController : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;
    
    private Tween _playButtonTween;

    private EntranceUIController _entranceUIController;

    [Inject]
    private void Construct(EntranceUIController entranceUIController)
    {
        _entranceUIController = entranceUIController;
        Show();
        _playButton.onClick.AddListener(PlayButtonClicked);
    }

    private void PlayButtonClicked()
    {
        _entranceUIController.Show();
        Hide();
    }

    public void Show()
    {
        _playButtonTween =_playButton.transform.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        _playButton.gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        _playButtonTween.Kill();
        _playButton.gameObject.SetActive(false);
    }

}
