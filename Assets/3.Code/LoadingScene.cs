using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{

    private void Start()
    {
        SceneManager.LoadScene("SeoLobbyScene2");
    }
}
