using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePage : Page
{
    private Button _startButton;
    private Button _collectionButton;
    private Button _quitButton;

    private void Awake()
    {
        Button[] buttons = GetComponentsInChildren<Button>();

        _startButton = buttons[0];
        _collectionButton = buttons[1];
        _quitButton = buttons[2];

        _startButton.onClick.AddListener(StartButtonClick);
        _collectionButton.onClick.AddListener(CollectionButtonClick);
        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    private void StartButtonClick()
    {
        PageManager.Instance.OpenPage<StagePage>();
    }
    private void CollectionButtonClick()
    {
        PageManager.Instance.OpenPage<CollectionPage>();
    }
    private void QuitButtonClick()
    {
        Application.Quit();
    }
}
