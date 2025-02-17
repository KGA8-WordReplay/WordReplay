using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class WordReplayManager : MonoBehaviour
{
    [Header("점수 관련 변수")]
    public int basicScore = 10;
    public int curScore;
    public int maxScore;

    //프로퍼티
    public string PreWord { get; set; }

    public bool IsEndGame { get; private set; }
    public WordReplayMainUI MainUI { get; private set; }

    //private
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
        GameStart();
    }

    private void GameStart()
    {
        _timer.OnTimer();
        MainUI.AddScore(curScore);
        MainUI.SetMaxScore(maxScore);
    }

    //단어 입력에 대한 처리
    public void HandleWordSubmission(string word)
    {
        PreWord = word;
        MainUI.UpdateWordDisplay(word, WordStorageManager.Instance.wordStorage.MyWordDict[word]);
        WordStorageManager.Instance.wordStorage.UsedWord.Add(word);

        _timer.MaxTimer();
        curScore += CalcScoreByLength(word);
        MainUI.AddScore(curScore);

        if (IsMaxScore())
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

    private int CalcScoreByLength(string word)
    {
        int bonus = (word.Length - 2) * 5;
        int result = basicScore + bonus;
        return result;
    }

    private bool IsMaxScore()
    {
        return curScore >= maxScore ? true : false;
    }

    private void Init()
    {
        MainUI = FindObjectOfType<WordReplayMainUI>();
        _autoMode = FindObjectOfType<AutoMode>();
        _timer = FindObjectOfType<Timer>();
    }
}
