using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public string stageName;
    public int achieveScore;
    public bool isLock = false;

    private Button _stageButton;
    private GameObject _lockGO;

    private void Awake()
    {
        _stageButton = GetComponent<Button>();
        _lockGO = transform.Find("Lock").gameObject;

        _stageButton.onClick.AddListener(StageButtonClick);
    }

    private void Start()
    {
        isLock = UserDataManager.Instance.IsStageClear(stageName);
        LockStage();
    }

    private void StageButtonClick()
    {
        SceneManager.LoadScene("WordReplayScene");
    }

    public void LockStage()
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
