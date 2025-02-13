using Seo;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WordStorage
{
    //전체 데이터 사전
    public Dictionary<string, string> EveryWordDict { get; private set; }
    //나의 데이터 사전
    public Dictionary<string, string> MyWordDict { get; private set; }
    //사용한 데이터 사전
    public List<string> UsedWord { get; private set; }

    public void AddUsedWord(string used)
    {
        UsedWord.Add(used);
    }

    public void Init()
    {
        //필드 초기화
        EveryWordDict = new Dictionary<string, string>();
        MyWordDict = new Dictionary<string, string>();
        UsedWord = new List<string>();

        //엑셀 파일 에러땜에 나중에 주석만 지우면 됌.
        //데이터 가공
        //List<Dictionary<string, object>> everyWordDict = CSVReader.Read("Word/EveryWord");
        List<Dictionary<string, object>> myWordDict = CSVReader.Read("Word/MyWord");

        //전체 데이터 사전 등록
        //EveryWordDict = ConvertToStringDictionary(everyWordDict);

        //나의 데이터 사전 등록
        MyWordDict = ConvertToStringDictionary(myWordDict);
    }

    private Dictionary<string, string> ConvertToStringDictionary(List<Dictionary<string, object>> data)
    {
        Dictionary<string, string> processedData = new Dictionary<string, string>();
        for (int i = 0; i < data.Count; i++)
        {
            string word = data[i]["어휘"].ToString();
            string explanation = data[i]["뜻풀이"].ToString();

            if (processedData.ContainsKey(word)) continue;
            processedData.Add(word, explanation);
        }
        return processedData;
    }
}
