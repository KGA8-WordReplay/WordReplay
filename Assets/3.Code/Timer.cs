using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float _curTime;
    private Slider _slider;

    private WordReplayManager _wordReplayManager;
    private Coroutine _timerCoroutine;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _wordReplayManager = FindObjectOfType<WordReplayManager>();
    }

    public void MaxTimer()
    {
        _curTime = 0f;
    }

    public void OnTimer()
    {
        _timerCoroutine = StartCoroutine(TimerCoroutine(_wordReplayManager.ScoreTracker.GetLimitedTime()));
    }

    //private IEnumerator TimerCoroutine()
    //{
    //    _curTime = 0;
    //    float limitedTime = _wordReplayManager.ScoreTracker.GetLimitedTime();

    //    while (_curTime <= limitedTime)
    //    {
    //        if (_wordReplayManager.IsEndGame) yield break;

    //        _curTime += Time.deltaTime;

    //        float progress = Mathf.Clamp01(_curTime / limitedTime);

    //        float value = Mathf.Lerp(1f, 0f, progress);

    //        _slider.value = value;
    //        //print($"타이머 작동 중 {value}");
    //        limitedTime = _wordReplayManager.ScoreTracker.GetLimitedTime();
    //        print($"제한 시간 {limitedTime}");
    //        yield return null;
    //    }
    //    //print("타이머 끝");
    //    _slider.value = 0f;

    //    _wordReplayManager.GameResult(false);
    //}

    public IEnumerator TimerCoroutine(float limitedTime)
    {
        _curTime = 0;
        print($"제한 시간 {limitedTime}");

        while (_curTime <= limitedTime)
        {
            if (_wordReplayManager.IsEndGame) yield break;

            _curTime += Time.deltaTime;

            float progress = Mathf.Clamp01(_curTime / limitedTime);

            float value = Mathf.Lerp(1f, 0f, progress);

            _slider.value = value;
            //print($"타이머 작동 중 {value}");
            yield return null;
        }
        //print("타이머 끝");
        _slider.value = 0f;

        _wordReplayManager.GameResult(false);
    }
}
