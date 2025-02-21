using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private UserData _userData;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == this)
        {
            Load();
        }
    }

    public void Save(int gold)
    {
        _userData.AddGold(gold);
        SaveUserData();
    }
    public void Save(string collectionName)
    {
        _userData.AddCollectionName(collectionName);
        SaveUserData();
    }
    public void SaveStageClear(string stage)
    {
        _userData.AddStageClear(stage);
        SaveUserData();
    }

    public bool IsStageClear(string stageName)
    {
        return _userData.IsStageClear(stageName);
    }

    private void SaveUserData()
    {
        string json = JsonConvert.SerializeObject(_userData);
        PlayerPrefs.SetString("userData", json);
        PlayerPrefs.Save();
        print("UserData가 저장됨: " + json);
    }

    private void Load()
    {
        //UserData가 있을 때
        if (PlayerPrefs.HasKey("userData"))
        {
            string json = PlayerPrefs.GetString("userData");
            _userData = JsonConvert.DeserializeObject<UserData>(json);
            print("저장된 UserData가 불러와짐.");
            _userData.Test(_userData);
        }
        //UserData가 없다면 생성해서 할당
        else
        {
            _userData = new UserData();
            print("새로운 UserData가 불러와짐.");
        }

        SaveStageClear("stage1");
    }
}
