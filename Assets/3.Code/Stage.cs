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

    private TextMeshProUGUI _achieveScoreText;

    private Button _stageButton;
    private GameObject _lockGO;

    private void Awake()
    {
        _stageButton = GetComponent<Button>();
        _lockGO = transform.Find("Lock").gameObject;

        _stageButton.onClick.AddListener(StageButtonClick);

        _achieveScoreText = GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    private void Start()
    {
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
