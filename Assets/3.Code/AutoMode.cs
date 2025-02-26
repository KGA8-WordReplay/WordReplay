using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoMode : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _backgroundRenderer;
    [SerializeField] private Sprite _backgroundAutoSource;
    private Sprite _backgroundOrigin;

    public float autoInterval;
    private WordReplayManager _wordReplayManager;

    private void Awake()
    {
        _wordReplayManager = FindObjectOfType<WordReplayManager>();
    }

    private void Start()
    {
        _backgroundOrigin = _backgroundRenderer.sprite;
    }

    #region 두음 법칙 로직
    private bool IsDueum(char letter)
    {
        return WordStorageManager.Instance.wordStorage.DueumDict.ContainsKey(letter);
    }

    private char ConvertToDueum(char lastLetter)
    {
        return WordStorageManager.Instance.wordStorage.DueumDict[lastLetter];
    }
    #endregion

    public IEnumerator AutoCoroutine()
    {
        WordStorage wordStorage = WordStorageManager.Instance.wordStorage;
        while (!_wordReplayManager.IsEndGame)
        {
            if (string.IsNullOrEmpty(_wordReplayManager.PreWord))
            {
                KeyValuePair<string, string> firstWord = wordStorage.MyWordDict.FirstOrDefault();
                _wordReplayManager.HandleWordSubmission(firstWord.Key, true);
            }
            else
            {
                print($"이전 단어: {_wordReplayManager.PreWord}");

                //이전 단어의 마지막 글자
                char lastLetter = _wordReplayManager.PreWord.LastOrDefault();

                List<string> findWords = new List<string>();

                //이전 단어 마지막 글자와 일치하는 단어List 검출
                foreach (string word in wordStorage.MyWordDict.Keys)
                {
                    if (wordStorage.UsedWord.Contains(word)) continue;

                    //이전 마지막 글자로 시작하는 단어가 있는지
                    if (word.FirstOrDefault().Equals(lastLetter))
                    {
                        findWords.Add(word);
                    }
                    //두음법칙으로 했을 때, 사용할 수 있는 단어가 있는지
                    else if (IsDueum(lastLetter))
                    {
                        char dueumLetter = ConvertToDueum(lastLetter);
                        if (word.FirstOrDefault().Equals(dueumLetter))
                        {
                            findWords.Add(word);
                        }
                    }
                }

                if (findWords.Count <= 0)
                {
                    _wordReplayManager.MainUI.onAuto = false;
                    _wordReplayManager.MainUI.AutoButtonColor();
                    print("이어붙일 단어가 없음");
                    _wordReplayManager.MainUI.inputText.interactable = true;
                    _backgroundRenderer.sprite = _backgroundOrigin;
                    yield break;
                }
                _backgroundRenderer.sprite = _backgroundAutoSource;

                string longestWord = "";
                //찾은 단어에서 제일 긴 단어 검출
                foreach (string word in findWords)
                {
                    if (word.Length >= longestWord.Length)
                    {
                        longestWord = word;
                    }
                }
                print($"다음 단어: {longestWord}");

                if (!_wordReplayManager.IsEndGame)
                {
                    _wordReplayManager.HandleWordSubmission(longestWord, true);
                }
            }
            yield return new WaitUntil(() => BlockManager.Instance.blockSpawnEnd);
            yield return new WaitForSeconds(1f);
        }
    }
}
