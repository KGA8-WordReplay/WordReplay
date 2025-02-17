using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class WordReplayManager : MonoBehaviour
{
    public string PreWord { get; set; }

    public bool IsEndGame { get; private set; }
    public WordReplayMainUI MainUI { get; private set; }
    private AutoMode _autoMode;
    private Timer _timer;

    //코루틴
    private Coroutine _autoCoroutine;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        _timer.OnTimer();
    }

    //단어 입력에 대한 처리
    public void HandleWordSubmission(string word)
    {
        PreWord = word;
        MainUI.UpdateWordDisplay(word, WordStorageManager.Instance.wordStorage.MyWordDict[word]);
        WordStorageManager.Instance.wordStorage.UsedWord.Add(word);

        _timer.MaxTimer();
    }

    public void AutoMode(bool onAuto)
    {
        if (onAuto)
        {
            MainUI.AutoButtonColor();
            _autoCoroutine = StartCoroutine(_autoMode.AutoCoroutine());
        }
        else
        {
            MainUI.AutoButtonColor();
            if (_autoCoroutine != null) StopCoroutine(_autoCoroutine);
            print("오토기능 멈춤");
        }
    }

    public void GameResult(bool isSuccess)
    {
        IsEndGame = true;
        if (isSuccess)
        {
            OnSuccess();
        }
        else
        {
            OnDefeat();
        }
    }

    private void OnSuccess()
    {
        Debug.Log("스테이지 클리어!");
    }

    private void OnDefeat()
    {
        Debug.Log("시간 초과로 졌음");
    }

    private void Init()
    {
        MainUI = FindObjectOfType<WordReplayMainUI>();
        _autoMode = FindObjectOfType<AutoMode>();
        _timer = FindObjectOfType<Timer>();
    }
}
