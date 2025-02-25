using Seo;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionDataManager : Singleton<CollectionDataManager>
{
    //실제 단어 리소스 파일의 이름이 들어가야함
    [SerializeField] private List<string> myWord;
    [SerializeField] private string col1;
    [SerializeField] private string col2;

    //PlayerPrefab에서 가져오는 string 값이 들어가야함
    public List<string> myWordNameList = new List<string>();

    private Dictionary<string, string> myWordNameDict = new Dictionary<string, string>();

    public int currentGold = 0;

    private void Start()
    {
        myWordNameList = UserDataManager.Instance.GetCollectionName();
        Init();
        InitGold();

        UserDataManager.Instance.goldAction += InitGold;

        print("CollectionDataManager에서 옴");
        foreach(var temp in myWordNameList)
        {
            print(temp);
        }
    }

    public Action OnDataLoaded;
    private void Init()
    {
        print("CollectionDataManager Init");
        if (myWordNameList == null)
        {
            print("콜렉션 리스트 없음");
            return;
        }
        foreach (var myWord in myWordNameList)
        {
            List<Dictionary<string, object>> tempkey = CSVReader.Read($"Word/MyWord/{myWord}");

            Dictionary<string, string> tempword = ConvertToStringDictionary(tempkey, col1, col2);

            WordStorageManager.Instance.wordStorage.AddMyWordDict(tempword);
        }
        OnDataLoaded?.Invoke();
    }

    private void InitGold()
    {
        currentGold = UserDataManager.Instance.GetGold();
    }

    //구매 성공 시
    public void PurchaseWordDict(string collectionName)
    {
        print($"구매 시 이름 : {collectionName}");
        //콜렉션의 UI상에 보이는 이름을 리소스파일 이름으로 변환
        //string value = myWordNameDict[collectionName];
        //리소스 파일을 전환
        List<Dictionary<string, object>> resource = CSVReader.Read($"Word/MyWord/{collectionName}");
        //그 전환한 파일을 Dict로 전환
        Dictionary<string, string> tempWord = ConvertToStringDictionary(resource, col1, col2);
        //마지막으로 내가 가지고 있는 단어에 추가
        WordStorageManager.Instance.wordStorage.AddMyWordDict(tempWord);

        UserDataManager.Instance.Save(collectionName);
        myWordNameList.Add(collectionName);
        print($"저장된 파일 이름 : {collectionName}");
    }

    private Dictionary<string, string> ConvertToStringDictionary(List<Dictionary<string, object>>data, string colName, string colName2)
    {
        Dictionary<string, string> processedData = new Dictionary<string, string>();
        for(int i = 0; i < data.Count; i++)
        {
            string word = data[i][colName].ToString();
            string explanation = data[i][colName2].ToString();

            if (processedData.ContainsKey(word)) continue;
            processedData.Add(word, explanation);
        }

        return processedData;
    }
}
