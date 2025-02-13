using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordReplayManager : MonoBehaviour
{
    public string PreWord { get; set; }

    public WordReplayMainUI mainUI { get; private set; }

    private AutoMode _autoMode;
    private Coroutine _autoCoroutine;

    private void Awake()
    {
        mainUI = FindObjectOfType<WordReplayMainUI>();
        _autoMode = FindObjectOfType<AutoMode>();

        WordStorage wordStorage = new WordStorage();
        wordStorage.Init();
    }

    public void AutoMode(bool onAuto)
    {
        if (onAuto)
        {
            mainUI.AutoButtonColor();
            _autoCoroutine = StartCoroutine(_autoMode.AutoCoroutine());
        }
        else
        {
            mainUI.AutoButtonColor();
            StopCoroutine(_autoCoroutine);
            print("오토기능 멈춤");
        }
    }
}
