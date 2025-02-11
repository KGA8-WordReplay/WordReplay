using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoMode : MonoBehaviour
{
    public string preWord;
    public float autoInterval;
    public bool isAuto = false;

    private string wordData = "바다,다리,리본,본능,능력,력사,사과,과일,일기,기차,차표,표시,시계,계획,획득,등록,녹색,색연필,필통,통장,장미,미소,소개,개구리,리듬,음악,악기,기술,술병,병원,원숭이,이야기,기사,사전,전등,등록,국가,가방,방송,송편,편지,지우개,게임,임무,무대,대문,문서,서랍,랍스터,터널,널판지,지갑,갑옷,옷장,장갑,갑자기,기관,완성,성공,공원,원인,인간,간식,식당,당근,근육,육체,체육,육상,상자,자동차,하늘,늘푸른,은하수,수영,영화,화산,산책,책상,상점,점수,수박,박물관,관광,광고,고양이,이불,불꽃,꽃병,병아리,리본,본질,질문,문제,제목,목표,표정,정답,답변,변화,화장실,실패";
    private List<string> _myWordsList = new List<string>();

    private SEOTest _seoTest;

    private void Awake()
    {
        _seoTest = FindObjectOfType<SEOTest>();

        string[] splitWords = wordData.Split(',');

        foreach (string word in splitWords)
        {
            _myWordsList.Add(word);
        }
    }

    private void Start()
    {
        StartCoroutine(AutoCoroutine());
    }

    private IEnumerator AutoCoroutine()
    {
        while (isAuto)
        {
            if (string.IsNullOrEmpty(preWord))
            {
                preWord = _myWordsList[Random.Range(0, _myWordsList.Count)];
                _seoTest.wordText.text = preWord;
            }
            else
            {
                print($"이전 단어: {preWord}");

                char lastLetter = preWord.LastOrDefault();
                print(lastLetter);
                string findWord = _myWordsList.Find(word => word.LastOrDefault().Equals(lastLetter));

            }
            //_seoTest.wordText.text = _myWordsList[Random.Range(0, _myWordsList.Count)];
            yield return new WaitForSeconds(autoInterval);
        }
    }
}
