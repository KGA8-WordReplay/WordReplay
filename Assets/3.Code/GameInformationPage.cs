using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInformationPage : MonoBehaviour
{
    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _confirmButton;

    public List<GameObject> informations = new List<GameObject>();
    private int currentIndex = 0;

    private void Start()
    {
        _prevButton?.onClick.AddListener(PrevButtonClick);
        _nextButton?.onClick.AddListener(NextButtonClick);
        UpdatePage();
    }

    private void OnEnable()
    {
        _confirmButton.onClick.AddListener(ConfirmButtonClick);
    }
    private void OnDisable()
    {
        _confirmButton.onClick.RemoveListener(ConfirmButtonClick);
    }

    private void ConfirmButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        Destroy(gameObject);
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

    private void NextButtonClick()
    {
        if (currentIndex < informations.Count - 1)
        {
            currentIndex++;
            UpdatePage();
            AudioManager.Instance.PlaySfx(Sfx.Button);
        }
    }

    private void UpdatePage()
    {
        // 모든 오브젝트 비활성화
        foreach (GameObject obj in informations)
        {
            obj.SetActive(false);
        }

        // 현재 인덱스의 오브젝트 활성화
        informations[currentIndex].SetActive(true);

        // 버튼 활성/비활성 설정
        _prevButton.interactable = currentIndex > 0;
        _nextButton.interactable = currentIndex < informations.Count - 1;

        if (currentIndex == informations.Count - 1)
        {
            _confirmButton.gameObject.SetActive(true);
        }
        else
        {
            _confirmButton.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _prevButton?.onClick.RemoveListener(PrevButtonClick);
        _nextButton?.onClick.RemoveListener(NextButtonClick);
    }
}
