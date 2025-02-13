using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoMode : MonoBehaviour
{
    public float autoInterval;

    private WordReplayManager _wordReplayManager;

    private void Awake()
    {
        _wordReplayManager = FindObjectOfType<WordReplayManager>();
    }

    public IEnumerator AutoCoroutine()
    {
        WordStorage wordStorage = WordManager.Instance.wordStorage;
        while (true)
        {
            if (string.IsNullOrEmpty(_wordReplayManager.PreWord))
            {
                KeyValuePair<string, string> firstWord = wordStorage.MyWordDict.FirstOrDefault();
                _wordReplayManager.PreWord = firstWord.Key;
                _wordReplayManager.mainUI.UpdateWordDisplay(firstWord.Key, firstWord.Value);
                wordStorage.UsedWord.Add(firstWord.Key);
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
                    //이전 마지막 글자로 시작하는 단어가 있는지 && 이미 사용한 단어는 제외
                    if (word.FirstOrDefault().Equals(lastLetter) && !wordStorage.UsedWord.Contains(word))
                    {
                        findWords.Add(word);
                    }
                }

                if (findWords.Count <= 0)
                {
                    _wordReplayManager.mainUI.onAuto = false;
                    _wordReplayManager.mainUI.AutoButtonColor();
                    print("이어붙일 단어가 없음");
                    yield break;
                }

                string longestWord = "";
                print($"longestWord의 처음 길이 값: {longestWord.Length}");
                //찾은 단어에서 제일 긴 단어 검출
                foreach (string word in findWords)
                {
                    if (word.Length >= longestWord.Length)
                    {
                        longestWord = word;
                    }
                }
                print($"다음 단어: {longestWord}");
                _wordReplayManager.PreWord = longestWord;
                _wordReplayManager.mainUI.UpdateWordDisplay(longestWord, wordStorage.MyWordDict[longestWord]);
                wordStorage.UsedWord.Add(longestWord);
            }
            yield return new WaitForSeconds(autoInterval);
        }
    }
}
