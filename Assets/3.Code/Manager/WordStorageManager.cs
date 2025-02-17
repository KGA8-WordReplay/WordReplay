using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordStorageManager : Singleton<WordStorageManager>
{
    public WordStorage wordStorage { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        wordStorage = new WordStorage();
        wordStorage.Init();
    }
}
