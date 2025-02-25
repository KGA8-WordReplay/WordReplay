using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public string stageName;
    public int achieveScore;

    private TextMeshProUGUI _stageNameText;
    private TextMeshProUGUI _achieveScoreText;

    private Button _stageButton;
    private GameObject _lockGO;

    private void Awake()
    {
        _stageButton = GetComponent<Button>();
        _lockGO = transform.Find("Lock").gameObject;

        _stageButton.onClick.AddListener(StageButtonClick);

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();

        _stageNameText = texts[0];
        _achieveScoreText = texts[1];
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(stageName)) _stageNameText.text = $"<{stageName}>";
        else
        {
            _stageNameText.text = "Coming Soon";
        }
        _achieveScoreText.text = $"클리어 점수 : {achieveScore}";
    }

    private void StageButtonClick()
    {
        SceneManager.LoadScene(stageName);
    }

    public void LockStage(bool isLock)
    {
        if (isLock)
        {
            _stageButton.interactable = false;
            _lockGO.SetActive(true);
        }
        else
        {
            _stageButton.interactable = true;
            _lockGO.SetActive(false);
        }
    }
}
