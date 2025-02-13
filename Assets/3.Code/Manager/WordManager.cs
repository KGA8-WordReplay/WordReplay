using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : Singleton<WordManager>
{
    public WordStorage wordStorage;

    private void Awake()
    {
        wordStorage = new WordStorage();
        wordStorage.Init();
    }
}
