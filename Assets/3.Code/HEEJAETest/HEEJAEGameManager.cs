using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
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
    private List<string> _meanList = new List<string>();
    private List<Dictionary<string, object>> _everyWord = new List<Dictionary<string, object>>();

    private void Awake()
    {
        confirmButton.onClick.AddListener(OnClickConfirmButton);
        _everyWord = HEEJAECSVReader.Read("EveryWord");
    }

    private void Start()
    {
        int i = 0;
        foreach (var row in _everyWord)
        {
            i++;
            string word = row["어휘"].ToString();
            print(row.ContainsKey("뜻풀이"));
            var a = row["뜻풀이"];
            print("0");
            string mean = row["뜻풀이"].ToString();
            print("1");

            _wordList.Add(word);
            print("2");

            _meanList.Add(mean);
            print("3");
            print(i);
        }
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
        foreach (var word in _wordList)
        {
            if (word != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void ShowMean(string inputText)
    {
        foreach(var word in _everyWord)
        {
            mean.text = word[inputText].ToString();
        }
    }

    private void StartWordChain()
    {

    }
}
