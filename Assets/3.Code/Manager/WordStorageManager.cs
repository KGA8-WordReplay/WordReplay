using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordStorageManager : Singleton<WordStorageManager>
{
    public string everyWord;
    public string myWord;
    public string duemWord;
    public string col1;
    public string col2;
    public string col3;
    public string col4;
    public WordStorage wordStorage { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        wordStorage = new WordStorage(everyWord, myWord, duemWord, col1, col2, col3, col4);
        wordStorage.Init();
    }

    //private void Start()
    //{
    //    wordStorage = new WordStorage(everyWord, myWord, duemWord, col1, col2, col3, col4);
    //    wordStorage.Init();
    //}
}
