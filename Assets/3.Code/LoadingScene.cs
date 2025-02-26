using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image loadingBar;

    private void Start()
    {
        StartCoroutine(LoadingCoroutine());
        StartCoroutine(LoadScene());
        //WordStorageManager.Instance.InitWordStorage();
    }

    private IEnumerator LoadingCoroutine()
    {
        while (true)
        {
            loadingText.text = "Loading.";
            yield return new WaitForSeconds(1f);
            loadingText.text = "Loading..";
            yield return new WaitForSeconds(1f);
            loadingText.text = "Loading...";
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator LoadScene()
    {
        float progress = 0f;
        yield return StartCoroutine(WordStorageManager.Instance.InitWordStorage());

        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("SeoLobbyScene2");
        asyncOper.allowSceneActivation = false;

        //진행도를 반영
        while (asyncOper.progress < 0.9f)
        {
            progress = Mathf.Lerp(progress, asyncOper.progress, Time.deltaTime * 2);
            UpdateProgress(progress);
            yield return null;
        }

        //로딩 완료
        UpdateProgress(1f);
        //로딩 완료 후 0.5초 대기
        yield return new WaitForSeconds(0.5f);

        asyncOper.allowSceneActivation = true;
    }

    private void UpdateProgress(float value)
    {
        loadingBar.fillAmount = value;
    }
}
