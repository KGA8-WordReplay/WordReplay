using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;

    private void Start()
    {
        StartCoroutine(LoadingCoroutine());
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
}
