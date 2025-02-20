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
    }
}
