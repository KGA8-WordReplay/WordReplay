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
    [SerializeField] private TextMeshProUGUI outputWord;
    [SerializeField] private TextMeshProUGUI explanationText;
    [SerializeField] private TextMeshProUGUI suggestionText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private string wholeWordTextName;
    [SerializeField] private string foodWordTextName;
    [SerializeField] private string wordName;
    [SerializeField] private string meanName;

    [SerializeField] private int count;

    public List<string> _everyWordList = new List<string>();
    public List<string> _foodWordList = new List<string>();
    private List<Dictionary<string, object>> _everyWordDic = new List<Dictionary<string, object>>();
    private List<Dictionary<string, object>> _foodWordDic = new List<Dictionary<string, object>>();

    private string _beforeWord;
    public bool hasSuggestion = false;

    public List<string> suggestionList = new List<string>();

    private string _typingWord;
    private string _preTypingWord = "";

    private int _currentSuggestionIndex = 0;

    private void Awake()
    {
        confirmButton.onClick.AddListener(OnClickConfirmButton);
        //input.onValueChanged.AddListener(OnInputChanged); //값이 바뀔때마다 검사
        input.onSubmit.AddListener(OnSubmit); //값이 바뀔때마다 검사
        _everyWordDic = HEEJAECSVReader.Read(wholeWordTextName);
        _foodWordDic = HEEJAECSVReader.Read(foodWordTextName);
    }

    private void Start()
    {
        //시작하면 처음 단어가 주어짐
        Init();

        //모든 단어 리스트 생성
        int i = 0;
        foreach (var row in _everyWordDic)
        {
            i++;
            string word = row[wordName].ToString();
            //string mean = row["뜻"].ToString();

            _everyWordList.Add(word);
            //_meanList.Add(mean);
            print(i);
        }

        foreach(var row in _foodWordDic)
        {
            string word = row[wordName].ToString();
            _foodWordList.Add(word);
        }
    }

    private void Update()
    {
        //string inputText = input.text;
        //string composition = Input.compositionString;
        _typingWord = input.text + Input.compositionString;
        //print($"찐 입력 : {_typingWord}");
        
        //입력값이 바뀔때만 확인
        if (_typingWord != _preTypingWord)
        {
            MatchWord(_typingWord);
            _preTypingWord = _typingWord;
        }

        //확인버튼 누를때
        if (Input.GetKeyDown(KeyCode.Return))
        {
            confirmButton.onClick.Invoke();
            OnSubmit(_typingWord);
        }

        //Tab키 누를때
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnClickTab();
        }

        //현재 조합중인 문자
       
        //print($"현재 조합중인 문자 : {composition}");

        ////입력된 문자
        //string currentInput = input.text;

        //if (!string.IsNullOrEmpty(composition))
        //{
        //    MatchWord(composition);
        //}
        //else
        //{
        //    suggestionText.text = "";
        //}
    }

    private void Init()
    {
        _beforeWord = "사과";
        outputWord.text = _beforeWord;
        explanationText.text = "사과 뜻";

        hasSuggestion = false;
    }

    //끝말잇기 로직이 들어가는 부분
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
                outputWord.text = inputText;
                ShowMean(inputText);
                suggestionText.text = " ";
            }
            //전체 단어에는 있지만 끝말잇기는 되지 않는 경우
            else
            {
                print("끝말잇기가 안됩니다");
                //suggestionText.text = "";
            }
        }
        else
        {
            explanationText.text = "없는 단어입니다.";
        }

        input.text = "";
        input.ActivateInputField();
    }

    private bool IsInputWordInList(string inputText)
    {
        foreach (var word in _everyWordList)
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
        foreach(var word in _everyWordDic)
        {
            if (word[wordName].ToString() == inputText)
            {
                string mean = word[meanName].ToString();
                this.explanationText.text = mean;
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
        string matchWord = _everyWordList.FirstOrDefault(word => word.StartsWith(input));

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
            suggestionText.text = "";
            //버퍼 뒤로 이동
            this.input.MoveTextEnd(false);
        }
    }

    private void OnClickTab()
    {
        if(suggestionList.Count > 0)
        {
            _currentSuggestionIndex = (_currentSuggestionIndex + 1) % suggestionList.Count;
            suggestionText.text = suggestionList[_currentSuggestionIndex];
            print($"{_currentSuggestionIndex}번째 추천 단어 : {suggestionText.text}");
        }
    }

    private void MatchWord(string composition)
    {
        print("매치단어 찾음");
        if (composition == "")
        {
            suggestionText.text = " ";
            return;
        }
        //단어 찾음
        //preSuggestion = _foodWordList.FirstOrDefault(word => word.StartsWith(composition));

        //맞는 단어 리스트로 만듦
        suggestionList = _foodWordList.Where(word => word.StartsWith(composition)).ToList();

        //추천 단어없으면 지정
        if (suggestionList.Count > 0)
        {
            _currentSuggestionIndex = 0;
            suggestionText.text = suggestionList[_currentSuggestionIndex];
            print($"처음 추천 단어 : {suggestionText.text}");
            hasSuggestion = true;
        }

        //print($"매치 된 단어 : {preSuggestion}");

        ////매치되는 단어가 있으면
        //if (!string.IsNullOrEmpty(preSuggestion))
        //{
        //    //추천 단어가 그 매치되는 단어로 바뀜
        //    suggestionText.text = preSuggestion;
        //    print("단어 찾음");
        //    hasSuggestion = true;
        //}
        //없으면
        else
        {
            suggestionText.text = "";
            print("단어 못찾음");
            hasSuggestion = false;
        }
    }
}
