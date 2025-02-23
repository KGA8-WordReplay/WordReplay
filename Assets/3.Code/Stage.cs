using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
	public string stageName;
	public int achieveScore;

	private Button _stageButton;
	private GameObject _lockGO;

	private void Awake()
	{
		_stageButton = GetComponent<Button>();
		_lockGO = transform.Find("Lock").gameObject;

		_stageButton.onClick.AddListener(StageButtonClick);
	}

	private void StageButtonClick()
	{
		SceneManager.LoadScene("WordReplayScene");
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
