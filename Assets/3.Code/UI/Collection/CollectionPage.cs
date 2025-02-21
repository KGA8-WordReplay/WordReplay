using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPage : Page
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button backButton;

    private void OnEnable()
    {
        backButton.onClick.AddListener(OnClickBackButton);
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        goldText.text = TempGoldSingleton.Instance.gold.ToString();
    }

    private void OnClickBackButton()
    {
        PageManager.Instance.OpenPage<TitlePage>();
    }
}
