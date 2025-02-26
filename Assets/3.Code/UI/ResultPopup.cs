using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPopup : Popup
{
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private Button _quitButton;
    private Action _quitAction;

    private void Awake()
    {
        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    public void SetPopup(string gold, Action action = null)
    {
        _goldText.text = gold;
        _quitAction = action;
    }

    private void QuitButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        PopupManager.Instance.PopupClose();
        _quitAction?.Invoke();
        print("나가기가 정상 작동 됌");
    }

    private void OnDestroy()
    {
        _quitButton.onClick.RemoveListener(QuitButtonClick);
    }
}
