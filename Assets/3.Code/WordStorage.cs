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
    //두음 데이터 사전
    public Dictionary<char, char> DueumDict { get; private set; }
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
        DueumDict = new Dictionary<char, char>();
        UsedWord = new List<string>();

        //엑셀 파일 에러땜에 나중에 주석만 지우면 됌.
        //데이터 가공
        //List<Dictionary<string, object>> everyWordDict = CSVReader.Read("Word/EveryWord");
        List<Dictionary<string, object>> myWordDict = CSVReader.Read("Word/TestWord");
        List<Dictionary<string, object>> dueumDict = CSVReader.Read("Word/Dueum");

        //전체 데이터 사전 등록
        //EveryWordDict = ConvertToStringDictionary(everyWordDict);

        //나의 데이터 사전 등록
        MyWordDict = ConvertToStringDictionary(myWordDict, "어휘", "뜻풀이");

        //두음 데이터 사전 등록
        DueumDict = ConvertToCharDictionary(dueumDict, "두음 적용 전", "두음 적용 후");
    }

    private Dictionary<string, string> ConvertToStringDictionary(List<Dictionary<string, object>> data, string colName, string colName2)
    {
        Dictionary<string, string> processedData = new Dictionary<string, string>();
        for (int i = 0; i < data.Count; i++)
        {
            string word = data[i][colName].ToString();
            string explanation = data[i][colName2].ToString();

            if (processedData.ContainsKey(word)) continue;
            processedData.Add(word, explanation);
        }
        return processedData;
    }

    private Dictionary<char, char> ConvertToCharDictionary(List<Dictionary<string, object>> data, string colName, string colName2)
    {
        Dictionary<char, char> processedData = new Dictionary<char, char>();
        for (int i = 0; i < data.Count; i++)
        {
            char word = data[i][colName].ToString()[0];
            char explanation = data[i][colName2].ToString()[0];

            if (processedData.ContainsKey(word)) continue;
            processedData.Add(word, explanation);
        }
        return processedData;
    }
}
