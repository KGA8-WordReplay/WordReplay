using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    [Header("UserData를 날리고 싶으시면 true를 하고 게임을 시작하세요.")]
    public bool isInit;

    public Action goldAction;

    private UserData _userData;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == this)
        {
            if (isInit) Init();
            Load();
        }
    }

    public void Init()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Save(int gold)
    {
        _userData.AddGold(gold);
        goldAction?.Invoke();
        SaveUserData();
    }
    public void Save(string collectionName)
    {
        _userData.AddCollectionName(collectionName);
        SaveUserData();
    }
    public void SaveStageUnlock(string stage)
    {
        _userData.AddStageUnlock(stage);
        SaveUserData();
    }

    public List<string> GetCollectionName()
    {
        return _userData.GetCollectionNames();
    }

    public bool IsStageLock(string stageName)
    {
        return !_userData.IsStageUnlock(stageName);
    }

    public int GetGold()
    {
        return _userData.GetGold();
    }

    public void SubGold(int gold)
    {
        _userData.SubGold(gold);
        goldAction?.Invoke();
        SaveUserData();
    }

    public void FalseNewGoStore()
    {
        _userData.isNewGoStore = false;
        SaveUserData();
    }
    public void FalseNewGoGame()
    {
        _userData.isNewGoGame = false;
        SaveUserData();
    }

    public bool IsNewGoStore()
    {
        return _userData.isNewGoStore;
    }

    public bool IsNewGoGame()
    {
        return _userData.isNewGoGame;
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

        SaveStageUnlock("stage1");
    }
}
