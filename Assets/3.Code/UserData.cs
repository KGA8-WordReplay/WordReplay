using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    [JsonProperty] private int gold;
    [JsonProperty] private List<string> collectionNames;
    [JsonProperty] private Dictionary<string, bool> stageClearDict;

    public UserData()
    {
        gold = 0;
        collectionNames = new List<string>();
        stageClearDict = new Dictionary<string, bool>();
    }

    //Add
    public void AddGold(int value) { gold += value; }

    public void AddCollectionName(string collectionName) { collectionNames.Add(collectionName); }

    public void AddStageClear(string stageName) { stageClearDict.Add(stageName, true); }

    //Sub
    public void SubGold(int value) { gold -= value; }

    //Get
    public int GetGold() { return gold; }

    public List<string> GetCollectionNames() { return collectionNames; }


    //bool
    public bool IsStageClear(string stageName)
    {
        return stageClearDict.ContainsKey(stageName);
    }

    public void Test(UserData userData)
    {
        Debug.Log($"골드량: {userData.gold}");
        Debug.Log("갖고 있는 사전");
        foreach (string collectionName in userData.collectionNames)
        {
            Debug.Log($"이름: {collectionName}");
        }
        Debug.Log("클리어한 스테이지");
        foreach (KeyValuePair<string, bool> pair in userData.stageClearDict)
        {
            Debug.Log($"이름: {pair.Key} 클리어: {pair.Value}");
        }
    }
}
