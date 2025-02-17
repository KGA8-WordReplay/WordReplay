using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class HEEJAEGameManager : MonoBehaviour
{
    private static HEEJAEGameManager _instance;
    public static HEEJAEGameManager Instance { get { return _instance; } }

    [SerializeField] private TMP_InputField input;
    [SerializeField] private TextMeshProUGUI outputWord;
    [SerializeField] private TextMeshProUGUI explanationText;
    [SerializeField] private TextMeshProUGUI suggestionText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private string wholeWordTextName;
    [SerializeField] private string foodWordTextName;
    [SerializeField] private string ruleOfHeadingTextName;
    [SerializeField] private string wordName;
    [SerializeField] private string meanName;

    [SerializeField] private int count;

    public List<string> _everyWordList = new List<string>();
    public List<string> _foodWordList = new List<string>();
    private List<Dictionary<string, object>> _everyWordDic = new List<Dictionary<string, object>>();
    private List<Dictionary<string, object>> _foodWordDic = new List<Dictionary<string, object>>();

    //두음법칙
    public Dictionary<char, char> _ruleOfHeading = new Dictionary<char, char>();

    private string _preWord; //이거는 전체 단어 관리자에서 전에 입력된 단어 가져올 거임
    public bool hasSuggestion = false;
    public string firstWord;

    //추천 단어 리스트
    public List<string> suggestionList = new List<string>();

    //치고있는 단어
    private string _typingWord;
    private string _preTypingWord = "";

    //현재 추천 단어
    private string _currentSuggetion = "";
    private string _privateCurrentSuggestion = "";

    private void Awake()
    {
        _instance = this;
        
        confirmButton.onClick.AddListener(OnClickConfirmButton);
        input.onValueChanged.AddListener(OnInputChanged); //값이 바뀔때마다 검사
        input.onSubmit.AddListener(OnSubmit); //값이 바뀔때마다 검사
        _everyWordDic = HEEJAECSVReader.Read(wholeWordTextName);
        _foodWordDic = HEEJAECSVReader.Read(foodWordTextName);

        List<Dictionary<string, object>> ruleOfHeadingList = HEEJAECSVReader.Read(ruleOfHeadingTextName);
        foreach(var ruleOfHeading in ruleOfHeadingList)
        {
            string before = ruleOfHeading["전"].ToString();
            string after = ruleOfHeading["후"].ToString();
            _ruleOfHeading[char.Parse(before)] = char.Parse(after);
        }
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
        _typingWord = input.text + Input.compositionString;

        if (_typingWord != _preTypingWord)
        {
            print("몇번");
            //매칭되는 단어 있으면
            if (HasMatchWord(_typingWord) == true)
            {
                //추천단어 설정하고
                SetCurrentSuggestion();

                //추천 단어가 있으면
                if (string.IsNullOrEmpty(_currentSuggetion) == false)
                {
                    BlockManager.Instance.MakeSuggestionBlock(_preWord, _typingWord, _currentSuggetion);
                }
            }
            //매칭되는 단어 없으면 그냥 삭제
            else
            {
                BlockManager.Instance.MakeBlock(_preWord, _typingWord);
            }
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
    }

    private void Init()
    {
        _preWord = firstWord;
        outputWord.text = _preWord;
        explanationText.text = "처음단어";
        //BlockManager.Instance.MakeLastWord(firstWord);

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
                //BlockManager.Instance.MakeLastWord(inputText);
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

    //private bool IsWordChainTrue(string input)
    //{
    //    char beforeLastWord = _beforeWord[_beforeWord.Length - 1];
    //    print($"이전 단어 끝글자 : {beforeLastWord}");
    //    char inputLastWord = input[0];
    //    print($"현재 단어 앞글자 : {inputLastWord}");

    //    if (beforeLastWord == inputLastWord)
    //    {
    //        print("끝말잇기 성공");
    //        _beforeWord = input;
    //        return true;
    //    }
    //    else
    //    {
    //        print("끝말잇기 실패");
    //        return false;
    //    }
    //}

    private bool IsWordChainTrue(string input)
    {
        //이전 글자의 끝글자
        char beforeLastWord = _preWord[_preWord.Length - 1];
        print($"이전 단어 끝글자 : {beforeLastWord}");

        //현재 글자의 앞글자
        char inputFirstWord = input[0];
        print($"현재 단어 앞글자 : {inputFirstWord}");

        ////현재 글자의 앞글자 분해
        //string disassemble = inputFirstWord.ToString().Normalize(NormalizationForm.FormD);
        //char vowels = disassemble[0];
        //print(vowels);

        if (beforeLastWord == inputFirstWord)
        {
            print("끝말잇기 성공");
            _preWord = input;
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
            //_currentSuggestionIndex = (_currentSuggestionIndex + 1) % suggestionList.Count;
            //suggestionText.text = suggestionList[_currentSuggestionIndex];
            //print($"{_currentSuggestionIndex}번째 추천 단어 : {suggestionText.text}");
            //SetCurrentSuggestion();
            SetCurrentSuggestion();
            BlockManager.Instance.MakeSuggestionBlock(_preWord, _typingWord, _currentSuggetion);
        }
    }

    //매치하는 단어가 있는지 확인
    private bool HasMatchWord(string composition)
    {
        if (composition == "")
        {
            suggestionText.text = "";
            suggestionList.Clear();
            return false;
        }
        if (_ruleOfHeading.ContainsKey(composition[0]))
        {
            print("두음법칙 적용됨");
            string secondWord = "";
            //첫번째 글자 치환 
            secondWord = composition.Replace(composition[0], _ruleOfHeading[composition[0]]);
            print($"변경됨 : {secondWord} ");

            suggestionList = _foodWordList.Where(word => word.StartsWith(secondWord) || word.StartsWith(composition)).ToList();
        }

        //두음 법칙이 성립 안할때
        else
        {
            //맞는 단어 리스트로 만듦
            suggestionList = _foodWordList.Where(word => word.StartsWith(composition)).ToList();
        }

        if (suggestionList.Count > 0)
        {
            print($"추천단어 설정 완료 : {suggestionList[0]}");
            return true;
        }
        else
        {
            print($"추천단어 없음");
            suggestionList.Clear();
            return false;
        }
        //int i = 0;
        //print("$$");
        ////단어가 없으면 빈칸으로 비워줌
        //if (composition == "")
        //{
        //    suggestionText.text = "";
        //    return false;
        //}

        //    i++;
        //    print($"변한 수 : {i}");
        //    //두음법칙 중에 입력중인 단어의 첫번재 단어가 있을경우
        //    if (_ruleOfHeading.ContainsKey(composition[0]))
        //    {
        //        print("두음법칙 적용됨");
        //        string secondWord = "";
        //        //첫번째 글자 치환 
        //        secondWord = composition.Replace(composition[0], _ruleOfHeading[composition[0]]);
        //        print($"변경됨 : {secondWord} ");

        //        suggestionList = _foodWordList.Where(word => word.StartsWith(secondWord) && word.StartsWith(composition)).ToList();
        //        print($"추천단어 : {suggestionList[0]}");
        //        return true;
        //    }

        //    else
        //    {
        //        //맞는 단어 리스트로 만듦
        //        suggestionList = _foodWordList.Where(word => word.StartsWith(composition)).ToList();
        //        return true;
        //    }

        //_preSuggestionList = new List<string>(suggestionList);
        ////추천 단어 있을떄
        //if (suggestionList.Count > 0)
        //{
        //    print("추천단어 있음");
        //    return true;
        //}

        //else
        //{
        //    return false;
        //}
    }

    private int _index = 0;

    private void SetCurrentSuggestion()
    {
        if (suggestionList.Count == 0)
        {
            _currentSuggetion = null;
            return;
        }

        _index++;
        int num = (_index % suggestionList.Count);
        _currentSuggetion = suggestionList[num];
        print($"현재 추천 단어 : {_currentSuggetion}");

        //BlockManager.Instance.MakeSuggestionBlock(_typingWord, _currentSuggetion);
    }
}
