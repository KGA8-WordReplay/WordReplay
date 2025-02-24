using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GiveupPopup : Popup
{
    public string nextSceneName;

    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private WordReplayManager _wordReplayManager;

    private void Awake()
    {
        _yesButton.onClick.AddListener(YesButtonClick);
        _noButton.onClick.AddListener(NoButtonClick);
    }

    private void Start()
    {
        _wordReplayManager = FindObjectOfType<WordReplayManager>();
    }

    private void YesButtonClick()
    {
        PopupManager.Instance.PopupClose();
        _wordReplayManager.GameResult(false);
    }

    private void NoButtonClick()
    {
        PopupManager.Instance.PopupClose();
    }

    private void OnDestroy()
    {
        _yesButton.onClick.RemoveListener(YesButtonClick);
        _noButton.onClick.RemoveListener(NoButtonClick);
    }
}
