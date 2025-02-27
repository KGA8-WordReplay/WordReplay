using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class WordReplayManager : MonoBehaviour
{
    public int winGold;

    public string stageName;
    public string nextScene;

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
        MainUI.inputText.ActivateInputField();
    }

    private void GameStart()
    {
        _timer.OnTimer();
        MainUI.AddScore(ScoreTracker.CurScore);
        MainUI.SetMaxScore(ScoreTracker.maxScore);

        AudioManager.Instance.PlayStageBgm(stageName);
    }

    //단어 입력에 대한 처리
    public void HandleWordSubmission(string word, bool Auto)
    {
        if (Auto)
        {
            StartCoroutine(MainUI.UpdateAutoDisplay(word, WordStorageManager.Instance.wordStorage.MyWordDict[word]));
        }
        else
        {
            MainUI.UpdateWordDisplay(word, WordStorageManager.Instance.wordStorage.EveryWordDict[word]);
        }

        PreWord = word;

        WordStorageManager.Instance.wordStorage.UsedWord.Add(word);

        _timer.MaxTimer();
        //InitTimer();
        ScoreTracker.CalcScoreByLength(word);
        MainUI.AddScore(ScoreTracker.CurScore);
        MainUI.UpdateScoreSlider(ScoreTracker.CurScore);

        if (ScoreTracker.IsMaxScore())
        {
            GameResult(true);
        }
    }

    public void AutoMode(bool onAuto)
    {
        if (onAuto)
        {
            AudioManager.Instance.PlaySfx(Sfx.AutoOn);
            MainUI.inputText.interactable = false;
            MainUI.AutoButtonColor();
            _autoCoroutine = StartCoroutine(_autoMode.AutoCoroutine());
        }
        else
        {
            AudioManager.Instance.PlaySfx(Sfx.AutoOff);
            _autoMode.ChangeBackground(false);
            MainUI.inputText.interactable = true;
            MainUI.inputText.ActivateInputField();
            MainUI.AutoButtonColor();
            if (_autoCoroutine != null) StopCoroutine(_autoCoroutine);
            print("오토기능 멈춤");
        }
    }

    //public void InitTimer()
    //{
    //    if (_timerCoroutine != null)
    //    {
    //        StopCoroutine(_timerCoroutine);
    //        print("타이머 멈춤");
    //    }
    //    _timerCoroutine = StartCoroutine(_timer.TimerCoroutine(ScoreTracker.GetLimitedTime()));
    //}

    public void GameResult(bool isSuccess)
    {
        IsEndGame = true;
        WordStorageManager.Instance.wordStorage.UsedWord.Clear();

        if (WordStorageManager.Instance.wordStorage.UsedWord.Count > 0)
        {
            print("게임이 끝났는데도 사용한 단어가 초기화가 안됐음");
        }
        else
        {
            print("게임이 끝나고 사용한 단어 초기화 완료.");
        }

        PopupManager.Instance.PopupCloseAll();
        AudioManager.Instance.PlayBgm(Bgm.None);

        if (isSuccess)
        {
            GameEndManager.Instance.Win();
            StartCoroutine(HandleWin());
            //OnSuccess();
        }
        else
        {
            GameEndManager.Instance.Lose();
            OnDefeat();
        }
    }

    private IEnumerator HandleWin()
    {
        yield return new WaitUntil(() => GameEndManager.Instance.WinAnimEnd);
        OnSuccess();
    }

    private void EndGame()
    {
        SceneManager.LoadScene(nextScene);
    }

    private void OnSuccess()
    {
        Debug.Log("스테이지 클리어!");
        int finalGold = winGold + ScoreTracker.CurScore;
        UserDataManager.Instance.Save(finalGold); //골드 저장
        UserDataManager.Instance.SaveStageUnlock(stageName); //스테이지 클리어 잠금 해제
        if (StageManager.Instance.IsNextStage(stageName))
        {
            StageManager.Instance.NextStageUnlock(stageName);
        }

        AudioManager.Instance.PlaySfx(Sfx.Win);
        PopupManager.Instance.PopupOpen<VictoryPopup>().SetPopup(finalGold.ToString(), EndGame);
    }

    private void OnDefeat()
    {
        Debug.Log("시간 초과로 졌음");
        UserDataManager.Instance.Save(ScoreTracker.CurScore); //골드 저장
        AudioManager.Instance.PlaySfx(Sfx.Lose);
        PopupManager.Instance.PopupOpen<DefeatPopup>().SetPopup(ScoreTracker.CurScore.ToString(), EndGame);
    }

    private void Init()
    {
        MainUI = FindObjectOfType<WordReplayMainUI>();
        _autoMode = FindObjectOfType<AutoMode>();
        _timer = FindObjectOfType<Timer>();
        ScoreTracker = FindObjectOfType<ScoreTracker>();
    }
}
