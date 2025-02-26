using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StagePage : Page
{
    public int channel;

    public List<GameObject> stages = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private GameObject _stagePivot;
    [SerializeField] private Button _nextButton;
    private Button _backButton;


    private void Awake()
    {
        Button[] buttons = GetComponentsInChildren<Button>();

        _backButton = buttons[0];

        _backButton.onClick.AddListener(BackButtonClick);

        _nextButton = buttons[1];
        _nextButton.onClick.AddListener(NextButtonClick);
    }

    private void Start()
    {
        _goldText.text = UserDataManager.Instance.GetGold().ToString();

        UserDataManager.Instance.goldAction += GoldAction;
    }

    private void GoldAction()
    {
        _goldText.text = UserDataManager.Instance.GetGold().ToString();
    }

    private void NextButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        if (channel + 1 < stages.Count)
        {
            stages[channel + 1].transform.SetAsLastSibling();
            channel++;
        }
        else
        {
            channel = 0;
            stages[channel].transform.SetAsLastSibling();
        }

        foreach (GameObject go in stages)
        {
            if (stages.IndexOf(go) == channel)
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }
    }

    public void OpenFirst()
    {
        channel = 0;
        stages[channel].transform.SetAsLastSibling();

        foreach (GameObject go in stages)
        {
            if (stages.IndexOf(go) == channel)
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }
    }

    private void BackButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        PageManager.Instance.OpenPage<TitlePage>();
    }

    private void OnDestroy()
    {
        UserDataManager.Instance.goldAction -= GoldAction;
    }
}
