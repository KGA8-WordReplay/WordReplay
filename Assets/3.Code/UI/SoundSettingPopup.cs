using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettingPopup : Popup
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private TextMeshProUGUI _bgmSoundText;
    [SerializeField] private TextMeshProUGUI _sfxSoundText;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        _bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        _sfxSlider.onValueChanged.AddListener(SetSfxVolume);

        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    private void QuitButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        PopupManager.Instance.PopupClose();
    }

    public void InitVolume()
    {
        SetBgmVolume(_bgmSlider.value);
        SetSfxVolume(_sfxSlider.value);
    }

    private void SetBgmVolume(float value)
    {
        int valueInt = Mathf.RoundToInt(value * 100);
        _bgmSoundText.text = valueInt.ToString();
        _audioMixer.SetFloat("bgm", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("bgm", _bgmSlider.value);
        PlayerPrefs.Save();
    }

    private void SetSfxVolume(float value)
    {
        int valueInt = Mathf.RoundToInt(value * 100);
        _sfxSoundText.text = valueInt.ToString();
        _audioMixer.SetFloat("sfx", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("sfx", _sfxSlider.value);
        PlayerPrefs.Save();
    }

    public bool CanLoadVolume()
    {
        if (PlayerPrefs.HasKey("bgm") && PlayerPrefs.HasKey("sfx"))
        {
            print("사운드세팅 로드 할 수 있음");
            return true;
        }
        else
        {
            print("사운드세팅 로드 할 수 없음");
            return false;
        }
    }

    public void LoadVolume()
    {
        float bgmV = PlayerPrefs.GetFloat("bgm");
        float sfxV = PlayerPrefs.GetFloat("sfx");

        _bgmSlider.value = bgmV;
        _sfxSlider.value = sfxV;

        SetBgmVolume(bgmV);
        SetSfxVolume(sfxV);
    }

    private void OnDestroy()
    {
        _bgmSlider.onValueChanged.RemoveListener(SetBgmVolume);
        _sfxSlider.onValueChanged.RemoveListener(SetSfxVolume);

        _quitButton.onClick.RemoveListener(QuitButtonClick);
    }
}
