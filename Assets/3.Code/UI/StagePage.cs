using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePage : Page
{
    private Button _backButton;

    private void Awake()
    {
        Button[] buttons = GetComponentsInChildren<Button>();

        _backButton = buttons[0];

        _backButton.onClick.AddListener(BackButtonClick);
    }

    private void BackButtonClick()
    {
        PageManager.Instance.OpenPage<TitlePage>();
    }
}
