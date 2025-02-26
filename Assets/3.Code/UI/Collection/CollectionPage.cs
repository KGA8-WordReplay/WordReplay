using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPage : Page
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button backButton;

    private int currentGold;

    private void OnEnable()
    {
        backButton.onClick.AddListener(OnClickBackButton);
        print("켜짐");
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
        print("꺼짐");
    }

    private void Start()
    {
        UserDataManager.Instance.goldAction += Init;
        Init();
    }

    private void Init()
    {
        goldText.text = CollectionDataManager.Instance.currentGold.ToString();
    }

    private void OnClickBackButton()
    {
        PageManager.Instance.OpenPage<TitlePage>();
    }
}
