using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class HEEJAEGameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private TextMeshProUGUI word;
    [SerializeField] private TextMeshProUGUI mean;
    [SerializeField] private TextMeshProUGUI suggestionText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private string textFileName;
    [SerializeField] private string wordName;
    [SerializeField] private string meanName;

    [SerializeField] private int count;

    public List<string> _wordList = new List<string>();
    //public List<string> _meanList = new List<string>();
    private List<Dictionary<string, object>> _everyWord = new List<Dictionary<string, object>>();

    private string _beforeWord;

    private void Awake()
    {
        confirmButton.onClick.AddListener(OnClickConfirmButton);
        input.onValueChanged.AddListener(OnInputChanged); //값이 바뀔때마다 검사
        input.onSubmit.AddListener(OnSubmit); //값이 바뀔때마다 검사
        _everyWord = HEEJAECSVReader.Read(textFileName);
    }

    private void Start()
    {
        //시작하면 처음 단어가 주어짐
        Init();

        int i = 0;
        foreach (var row in _everyWord)
        {
            i++;
            string word = row[wordName].ToString();
            //string mean = row["뜻"].ToString();

            _wordList.Add(word);
            //_meanList.Add(mean);
            print(i);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            confirmButton.onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnSubmit(input.text);
        }
        //if (!string.IsNullOrEmpty(Input.compositionString))
        //{
        //    Debug.Log($"조합 중 글자: {Input.compositionString}");
        //}
        //else
        //{
        //    Debug.Log($"완성된 텍스트: {input.text}");
        //}
    }

    private void Init()
    {
        _beforeWord = "사과";
        word.text = _beforeWord;
        mean.text = "사과 뜻";
    }

    //확인 버튼 누르면 단어가 있는지 확인 해야함
    private void OnClickConfirmButton()
    {
        //확인 버튼 누르면 확인하는 순서
        //1. 전체 단어에 있는 단어인지 확인
        //2. 끝말잇기가 되는지 확인
        string inputText = input.text;
        if (IsInputWordInList(inputText))
        {
            //전체 단어에도 있으면서 끝말잇기도 되는 경우
            if (IsWordChainTrue(inputText))
            {
                word.text = inputText;
                ShowMean(inputText);
            }
            //전체 단어에는 있지만 끝말잇기는 되지 않는 경우
            else
            {
                print("끝말잇기가 안됩니다");
            }
        }
        else
        {
            mean.text = "없는 단어입니다.";
        }

        input.text = "";
        input.ActivateInputField();
    }

    private bool IsInputWordInList(string inputText)
    {
        foreach (var word in _wordList)
        {
            if (word == inputText)
            {
                print("있는 단어입니다");
                return true;
            }
        }
        print("없는 단어입니다.");
        return false;
    }

    private bool IsWordChainTrue(string input)
    {
        char beforeLastWord = _beforeWord[_beforeWord.Length - 1];
        print($"이전 단어 끝글자 : {beforeLastWord}");
        char inputLastWord = input[0];
        print($"현재 단어 앞글자 : {inputLastWord}");

        if (beforeLastWord == inputLastWord)
        {
            print("끝말잇기 성공");
            _beforeWord = input;
            return true;
        }
        else
        {
            print("끝말잇기 실패");
            return false;
        }
    }

    private void ShowMean(string inputText)
    {
        foreach(var word in _everyWord)
        {
            if (word[wordName].ToString() == inputText)
            {
                string mean = word[meanName].ToString();
                this.mean.text = mean;
                return;
            }
        }
    }

    //input안에 값이 바뀔때 마다 실행
    private void OnInputChanged(string input)
    {
        //버퍼에 아무것도 없으면
        if (string.IsNullOrEmpty(input))
        {
            //비우기
            suggestionText.text = "";
            return;
        }

        //뭔가 있다면 그 단어로 시작하는 단어 찾기
        string matchWord = _wordList.FirstOrDefault(word => word.StartsWith(input));

        //매치되는 단어가 있으면
        if (!string.IsNullOrEmpty(matchWord))
        {
            suggestionText.text = matchWord;
        }
        //없으면
        else
        {
            suggestionText.text = "";
        }
    }

    private void OnSubmit(string input)
    {
        if (!string.IsNullOrEmpty(suggestionText.text))
        {
            this.input.text = suggestionText.text;
            //버퍼 뒤로 이동
            this.input.MoveTextEnd(false);
        }
    }
}
