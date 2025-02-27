using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StagePage : Page
{
    public int currentIndex;

    public List<GameObject> stages = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private GameObject _stagePivot;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;


    private void Awake()
    {

        _backButton.onClick.AddListener(BackButtonClick);
        _prevButton.onClick.AddListener(PrevButtonClick);
        _nextButton.onClick.AddListener(NextButtonClick);
    }


    private IEnumerator Start()
    {
        _goldText.text = UserDataManager.Instance.GetGold().ToString();

        UserDataManager.Instance.goldAction += GoldAction;

        UpdatePage();
        yield return null;
    }
    private void PrevButtonClick()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdatePage();
            AudioManager.Instance.PlaySfx(Sfx.Button);
        }
    }
    private void GoldAction()
    {
        _goldText.text = UserDataManager.Instance.GetGold().ToString();
    }

    private void NextButtonClick()
    {
        if (currentIndex < stages.Count - 1)
        {
            currentIndex++;
            UpdatePage();
            AudioManager.Instance.PlaySfx(Sfx.Button);
        }
    }
    private void UpdatePage()
    {
        // 모든 오브젝트 비활성화
        foreach (GameObject obj in stages)
        {
            obj.SetActive(false);
        }

        // 현재 인덱스의 오브젝트 활성화
        stages[currentIndex].SetActive(true);

        // 버튼 활성/비활성 설정
        _prevButton.interactable = currentIndex > 0;
        _nextButton.interactable = currentIndex < stages.Count - 1;
    }

    private void BackButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        PageManager.Instance.OpenPage<TitlePage>();
    }

    private void OnDestroy()
    {
        UserDataManager.Instance.goldAction -= GoldAction;
        _backButton.onClick.RemoveListener(BackButtonClick);
        _prevButton.onClick.RemoveListener(PrevButtonClick);
        _nextButton.onClick.RemoveListener(NextButtonClick);
    }
}
