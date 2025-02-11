using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HEEJAEGameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private TextMeshProUGUI word;
    [SerializeField] private TextMeshProUGUI mean;
    [SerializeField] private Button confirmButton;
    [SerializeField] private int _count;

    private List<string> _wordList = new List<string>();
    private List<Dictionary<string, object>> _everyWord = new List<Dictionary<string, object>>();

    private void Awake()
    {
        confirmButton.onClick.AddListener(OnClickConfirmButton);
    }

    private void Start()
    {
        _everyWord = HEEJAECSVReader.Read("EveryWord");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            confirmButton.onClick.Invoke();
        }
    }

    //확인 버튼 누르면 단어가 있는지 확인 해야함
    private void OnClickConfirmButton()
    {
        //있는 단어이면
        if (CheckInputWordInList(input.text))
        {
            ShowMean(input.text);
        }
        else
        {
            mean.text = "없는 단어입니다.";
        }

        if (input.text != null)
        {
            StartWordChain();
        }

        input.text = "";
        input.ActivateInputField();
    }
    private bool CheckInputWordInList(string inputText)
    {
        foreach (var word in _everyWord)
        {
            if (word[inputText] != null)
            {
                return 
            }
        }
    }

    private void ShowMean(string inputText)
    {
        string mean = _everyWord[inputText];
        this.mean.text = mean;
    }

    private void StartWordChain()
    {

    }
}
