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
        //if (Instance == this)
        //{
        //    wordStorage = new WordStorage(everyWord, myWord, duemWord, col1, col2, col3, col4);
        //    wordStorage.Init();
        //}
    }

    public IEnumerator InitWordStorage()
    {
        print("전체 단어 초기화 중");
        wordStorage = new WordStorage(everyWord, myWord, duemWord, col1, col2, col3, col4);
        wordStorage.Init();

        yield return null;
    }
}
