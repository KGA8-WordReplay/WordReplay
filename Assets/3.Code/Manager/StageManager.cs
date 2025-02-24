using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    public List<string> stageList = new List<string>();
    public string lobbySceneName;


    private void Start()
    {
        Init();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetActiveScene().name == lobbySceneName)
        {
            Init();
        }
    }

    private void Init()
    {
        stageList.Clear();

        Stage[] stages = FindObjectOfType<StagePage>().GetComponentsInChildren<Stage>();

        for (int i = 0; i < stages.Length; i++)
        {
            Stage stage = stages[i];
            bool isLocked = UserDataManager.Instance.IsStageLock(stage.stageName);

            stage.LockStage(isLocked);
            stageList.Add(stage.stageName);
        }
    }

    public bool IsNextStage(string stage)
    {
        int index = stageList.IndexOf(stage);
        return index >= 0 && index + 1 < stageList.Count;
    }

    public void NextStageUnlock(string stage)
    {
        int index = stageList.IndexOf(stage) + 1;
        UserDataManager.Instance.SaveStageUnlock(stageList[index]);
    }
}
