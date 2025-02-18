using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class WordReplayManager : MonoBehaviour
{

    //프로퍼티
    public string PreWord { get; set; }

    public bool IsEndGame { get; private set; }
    public WordReplayMainUI MainUI { get; private set; }
    public ScoreTracker ScoreTracker { get; private set; }

    //private
    private AutoMode _autoMode;
    private Timer _timer;

    //코루틴
    private Coroutine _autoCoroutine;
    private Coroutine _timerCoroutine;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        GameStart();
    }

    private void GameStart()
    {
        _timer.OnTimer();
        MainUI.AddScore(ScoreTracker.CurScore);
        MainUI.SetMaxScore(ScoreTracker.maxScore);
    }

    //단어 입력에 대한 처리
    public void HandleWordSubmission(string word, bool Auto)
    {
        if (Auto)
        {
            MainUI.UpdateWordDisplay(word, WordStorageManager.Instance.wordStorage.MyWordDict[word]);
        }
        else
        {
            MainUI.UpdateWordDisplay(word, WordStorageManager.Instance.wordStorage.EveryWordDict[word]);
        }

        PreWord = word;

        WordStorageManager.Instance.wordStorage.UsedWord.Add(word);

        //_timer.MaxTimer();
        InitTimer();
        ScoreTracker.CalcScoreByLength(word);
        MainUI.AddScore(ScoreTracker.CurScore);

        if (ScoreTracker.IsMaxScore())
        {
            GameResult(true);
        }
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

    public void InitTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            print("타이머 멈춤");
        }
        _timerCoroutine = StartCoroutine(_timer.TimerCoroutine(ScoreTracker.GetLimitedTime()));
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
        ScoreTracker = FindObjectOfType<ScoreTracker>();
    }
}
