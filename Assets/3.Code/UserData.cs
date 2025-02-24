using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    [JsonProperty] private int gold;
    [JsonProperty] private List<string> collectionNames;
    [JsonProperty] private List<string> stagesUnlock;

    public UserData()
    {
        gold = 0;
        collectionNames = new List<string>();
        stagesUnlock = new List<string>();
    }

    //Add
    public void AddGold(int value) { gold += value; }

    public void AddCollectionName(string collectionName)
    {
        if (!collectionNames.Contains(collectionName))
            collectionNames.Add(collectionName);
    }

    public void AddStageUnlock(string stageName)
    {
        if (!stagesUnlock.Contains(stageName))
        {
            stagesUnlock.Add(stageName);
        }
    }

    //Sub
    public void SubGold(int value) { gold -= value; }

    //Get
    public int GetGold() { return gold; }

    public List<string> GetCollectionNames() { return collectionNames; }


    //bool
    public bool IsStageUnlock(string stageName)
    {
        return stagesUnlock.Contains(stageName);
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
        foreach (string stageName in userData.stagesUnlock)
        {
            Debug.Log($"스테이지: {stageName}");
        }
    }
}
