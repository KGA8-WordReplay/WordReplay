using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : Singleton<WordManager>
{
    public WordStorage wordStorage;

    protected override void Awake()
    {
        base.Awake();
        wordStorage = new WordStorage();
        wordStorage.Init();
    }
}
