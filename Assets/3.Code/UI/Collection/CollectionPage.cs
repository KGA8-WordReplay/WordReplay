using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPage : Page
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button backButton;

    //private int currentGold;

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
        Init();

        UserDataManager.Instance.goldAction += Init;
        //CollectionDataManager.Instance.goldChange += Init;
        //Init();
    }

    private void Init()
    {
        goldText.text = UserDataManager.Instance.GetGold().ToString();
    }

    private void OnClickBackButton()
    {
        PageManager.Instance.OpenPage<TitlePage>();
    }
}
