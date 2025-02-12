using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoMode : MonoBehaviour
{
    public float autoInterval;

    private string wordData = "바다,다리,리본,본능,능력,력사,사과,과일,일기,기차,차표,표시,시계,계획,획득,등록,녹색,색연필,필통,통장,장미,미소,소개,개구리,리듬,음악,악기,기술,술병,병원,원숭이,이야기,기사,사전,전등,등록,국가,가방,방송,송편,편지,지우개,게임,임무,무대,대문,문서,서랍,랍스터,터널,널판지,지갑,갑옷,옷장,장갑,갑자기,기관,완성,성공,공원,원인,인간,간식,식당,당근,근육,육체,체육,육상,상자,자동차,하늘,늘푸른,은하수,수영,영화,화산,산책,책상,상점,점수,수박,박물관,관광,광고,고양이,이불,불꽃,꽃병,병아리,리본,본질,질문,문제,제목,목표,표정,정답,답변,변화,화장실,실패";
    private Dictionary<string, int> _myWordDict = new Dictionary<string, int>();

    private WordReplayManager _wordReplayManager;

    private void Awake()
    {
        _wordReplayManager = FindObjectOfType<WordReplayManager>();

        string[] splitWords = wordData.Split(',');

        foreach (string word in splitWords)
        {
            if (_myWordDict.ContainsKey(word)) continue;
            _myWordDict.Add(word, 0);
        }
    }

    public IEnumerator AutoCoroutine()
    {
        while (true)
        {
            if (string.IsNullOrEmpty(_wordReplayManager.PreWord))
            {
                KeyValuePair<string, int> firstWord = _myWordDict.First();
                _wordReplayManager.PreWord = firstWord.Key;
                _wordReplayManager.mainUI.outputText.text = firstWord.Key;
                _myWordDict[firstWord.Key] = 1;
            }
            else
            {
                print($"이전 단어: {_wordReplayManager.PreWord}");

                //이전 단어의 마지막 글자
                char lastLetter = _wordReplayManager.PreWord.LastOrDefault();

                List<string> findWords = new List<string>();
                //이전 단어 마지막 글자와 일치하는 단어List 검출
                foreach (string word in _myWordDict.Keys)
                {
                    //이전 마지막 글자로 시작하는 단어가 있는지 && 이미 사용한 단어는 제외
                    if (word.FirstOrDefault().Equals(lastLetter) && _myWordDict[word] != 1)
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
                _wordReplayManager.mainUI.outputText.text = longestWord;

                _wordReplayManager.PreWord = longestWord;
                _myWordDict[longestWord] = 1;
            }
            yield return new WaitForSeconds(autoInterval);
        }
    }
}
