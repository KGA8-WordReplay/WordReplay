using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitlePage : Page
{
    [SerializeField] private GameObject _gameInfoPrefab;
    [SerializeField] private GameObject _storeInfoPrefab;

    [SerializeField] private SoundSettingPopup _soundSettingPopup;
    [SerializeField] private Button _soundSettingButton;
    private Button _startButton;
    private Button _collectionButton;
    private Button _informationButton;
    private Button _quitButton;

    private void Start()
    {
        Button[] buttons = transform.Find("Buttons").GetComponentsInChildren<Button>();

        _startButton = buttons[0];
        _collectionButton = buttons[1];
        _informationButton = buttons[2];
        _quitButton = buttons[3];

        _startButton.onClick.AddListener(StartButtonClick);
        _collectionButton.onClick.AddListener(CollectionButtonClick);
        _informationButton.onClick.AddListener(InformationButtonClick);
        _soundSettingButton.onClick.AddListener(SoundSettingButtonClick);
        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    public void Init()
    {
        if (_soundSettingPopup.CanLoadVolume())
        {
            _soundSettingPopup.LoadVolume();
        }
        else
        {
            _soundSettingPopup.InitVolume();
        }
    }


    private void StartButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        if (UserDataManager.Instance.IsNewGoGame())
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            Instantiate(_gameInfoPrefab, canvas.transform);
            UserDataManager.Instance.FalseNewGoGame();
        }
        PageManager.Instance.OpenPage<StagePage>().UpdatePage();
    }
    private void CollectionButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        if (UserDataManager.Instance.IsNewGoStore())
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            Instantiate(_storeInfoPrefab, canvas.transform);
            UserDataManager.Instance.FalseNewGoStore();
        }
        PageManager.Instance.OpenPage<CollectionPage>();
    }
    private void InformationButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        PageManager.Instance.OpenPage<InformationPage>();
    }
    private void SoundSettingButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        PopupManager.Instance.PopupOpen<SoundSettingPopup>();
    }

    private void QuitButtonClick()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        _startButton?.onClick.RemoveListener(StartButtonClick);
        _collectionButton?.onClick.RemoveListener(CollectionButtonClick);
        _informationButton?.onClick.RemoveListener(InformationButtonClick);
        _soundSettingButton?.onClick.RemoveListener(SoundSettingButtonClick);
        _quitButton.onClick?.RemoveListener(QuitButtonClick);
    }
}
