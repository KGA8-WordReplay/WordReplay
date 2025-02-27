using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitlePage : Page
{
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private SoundSettingPopup _soundSettingPopup;
    private Button _startButton;
    private Button _collectionButton;
    private Button _informationButton;
    private Button _soundSettingButton;
    private Button _quitButton;

    private void Awake()
    {
        Button[] buttons = transform.Find("Buttons").GetComponentsInChildren<Button>();

        _startButton = buttons[0];
        _collectionButton = buttons[1];
        _informationButton = buttons[2];
        _soundSettingButton = buttons[3];
        _quitButton = buttons[4];

        _startButton.onClick.AddListener(StartButtonClick);
        _collectionButton.onClick.AddListener(CollectionButtonClick);
        _informationButton.onClick.AddListener(InformationButtonClick);
        _soundSettingButton.onClick.AddListener(SoundSettingButtonClick);
        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    private void Start()
    {
        if (_soundSettingPopup.CanLoadVolume())
        {
            _soundSettingPopup.LoadVolume();
        }
        else
        {
            _soundSettingPopup.InitVolume();
        }

        _goldText.text = UserDataManager.Instance.GetGold().ToString();

        UserDataManager.Instance.goldAction += GoldAction;
    }

    private void GoldAction()
    {
        _goldText.text = UserDataManager.Instance.GetGold().ToString();
    }

    private void StartButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        PageManager.Instance.OpenPage<StagePage>().UpdatePage();
    }
    private void CollectionButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
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
        _startButton.onClick.RemoveListener(StartButtonClick);
        _collectionButton.onClick.RemoveListener(CollectionButtonClick);
        _informationButton.onClick.RemoveListener(InformationButtonClick);
        _soundSettingButton.onClick.RemoveListener(SoundSettingButtonClick);
        _quitButton.onClick.RemoveListener(QuitButtonClick);

        UserDataManager.Instance.goldAction -= GoldAction;
    }
}
