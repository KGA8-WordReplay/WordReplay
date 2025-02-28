using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HEEJAEGameManager : MonoBehaviour
{
    private static HEEJAEGameManager _instance;
    public static HEEJAEGameManager Instance { get { return _instance; } }

    [SerializeField] private TMP_InputField input;
    [SerializeField] private TextMeshProUGUI outputWord;
    [SerializeField] private TextMeshProUGUI explanationText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button suggestionButton;
    [SerializeField] private string wholeWordTextName;
    [SerializeField] private string foodWordTextName;
    [SerializeField] private string ruleOfHeadingTextName;
    [SerializeField] private string wordName;
    [SerializeField] private string meanName;
    [SerializeField] private int count;

    [Header("여기에 제시어 입력")]
    [SerializeField] private List<string> stage1 = new List<string> { "바람결", "햇살", "비행선", "구름층", "창공", "일출", "대기권", "공중전", "새벽" };
    [SerializeField] private List<string> stage2 = new List<string> { "조개껍질", "해조류", "바닷물", "백사장", "파도소리", "갯벌", "해안선", "물결", "석양", "망망대해" };
    [SerializeField] private List<string> stage3 = new List<string> { "산호초", "바다", "거북", "야자수", "파도타기", "모래알", "해변", "모래", "서핑보드", "해양생물", "진주조개" };
    [SerializeField] private List<string> stage4 = new List<string> { "원시림", "동굴벽화", "정글", "탐험", "수풀길", "울창", "폭포수", "비단뱀", "나무다리", "수목원" };
    [SerializeField] private List<string> stage5 = new List<string> { "능선", "운무", "절벽", "산골짜기", "고원지대", "석양", "바위산", "등산길", "정상석", "안개구름" };
    [SerializeField] private List<string> stage6 = new List<string> { "초승달", "별자리", "운석군", "천문대", "은하수", "성운", "밤하늘", "유성우", "밤바람", "반딧불" };
    [SerializeField] private List<string> stage7 = new List<string> { "빗소리", "우산", "젖은길", "장마철", "물웅덩이", "빗줄기", "구름층", "안개비", "빗방울", "호수면" };
    [SerializeField] private List<string> stage8 = new List<string> { "번개", "암흑", "전력선", "폭우", "돌풍", "나뭇잎", "줄기", "벼락구름", "울림소리" };
    [SerializeField] private List<string> stage9 = new List<string> { "로켓발사", "성층권", "대기권", "무중력", "우주선", "출발선", "행성" };
    [SerializeField] private List<string> stage10 = new List<string> { "은하", "블랙홀", "행성군", "우주정거장", "암흑물질", "초신성", "태양풍", "우주망원경", "무한" };

    //임시 참조
    [SerializeField] private WordReplayManager wordReplayManager;

    private Dictionary<string, List<string>> startDict = new Dictionary<string, List<string>>();
    private List<string> startSuggestionList = new List<string>();

    private List<string> _foodWordList = new List<string>();

    //두음법칙
    public Dictionary<char, char> _ruleOfHeading = new Dictionary<char, char>();

    //private string _preWord; //이거는 전체 단어 관리자에서 전에 입력된 단어 가져올 거임
    public bool hasSuggestion = false;
    public string firstWord;

    //추천 단어 리스트
    public List<string> suggestionList = new List<string>();

    //치고있는 단어
    private string _typingWord;
    private string _preTypingWord = "";

    //현재 추천 단어
    public string _currentSuggetion = "";
    private string _privateCurrentSuggestion = "";

    //현재 씬
    private string _sceneName = "";

    public bool isOn = true;
    private void Awake()
    {
        _instance = this;

        confirmButton.onClick.AddListener(OnClickConfirmButton);
        suggestionButton.onClick.AddListener(OnClickSuggestionButton);
        input.onValueChanged.AddListener(OnInputChanged); //값이 바뀔때마다 검사
        input.onSubmit.AddListener(OnSubmit); //값이 바뀔때마다 검사

        //씬 이름 가져오기
        _sceneName = SceneManager.GetActiveScene().name;

        isOn = true;
        startDict = new Dictionary<string, List<string>>()
        {
            { "stage1", stage1},
            { "stage2", stage2},
            { "stage3", stage3},
            { "stage4", stage4},
            { "stage5", stage5},
            { "stage6", stage6},
            { "stage7", stage7},
            { "stage8", stage8},
            { "stage9", stage9},
            { "stage10", stage10},
        };

    }

    private void Start()
    {
        //시작하면 처음 단어가 주어짐
        Init();

        //모든 단어 리스트 생성
        foreach (var row in WordStorageManager.Instance.wordStorage.MyWordDict)
        {
            _foodWordList.Add(row.Key);
        }
        _ruleOfHeading = WordStorageManager.Instance.wordStorage.DueumDict;
    }

    private void Update()
    {
        _typingWord = input.text + Input.compositionString;

        //_typingWord가 바뀔 때 마다 들어옴
        if (_typingWord != _preTypingWord)
        {
            //Backspace키 누를때
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                AudioManager.Instance.PlaySfx(Sfx.BackspaceButton);
            }
            else
            {
                AudioManager.Instance.PlaySfx(Sfx.Typing2);

            }
            print("바뀜");
            //매칭되는 단어 있으면
            if (isOn == true && HasMatchWord(_typingWord) == true)
            {
                //추천단어 설정하고
                SetCurrentSuggestion();

                //추천 단어가 있고 전 단어에 이어지면
                if (string.IsNullOrEmpty(_currentSuggetion) == false && (!WordStorageManager.Instance.wordStorage.UsedWord.Contains(_currentSuggetion)))
                {
                    BlockManager.Instance.MakeSuggestionBlock(wordReplayManager.PreWord, _typingWord, _currentSuggetion);
                }
                else
                {
                    BlockManager.Instance.MakeBlock(wordReplayManager.PreWord, _typingWord);
                }
            }
            //매칭되는 단어 없으면 그냥 삭제
            else
            {
                _currentSuggetion = null;
                suggestionList.Clear();

                BlockManager.Instance.MakeBlock(wordReplayManager.PreWord, _typingWord);
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
        //시작하는 제시어 단어 리스트 가져오기
        if (startDict.TryGetValue(_sceneName, out List<string> suggestions))
        {
            startSuggestionList = suggestions;
        }
        else
        {
            Debug.LogError("씬 이름이 잘못 됨");
        }

        int randomNum = Random.Range(0, startSuggestionList.Count);
        string startSuggestion = startSuggestionList[randomNum];

        wordReplayManager.PreWord = startSuggestion;
        //print($"wordReplayManager.PreWord의 단어 : {wordReplayManager.PreWord}");
        //print($"firstWord의 단어 : {firstWord}");
        ////임시로 이전단어 넘김
        BlockManager.Instance.MakeFirstBlock(startSuggestion);
        print($"처음 단어 : {startSuggestion}");
        //outputWord.text = _preWord;
        explanationText.text = "처음단어"; // -> 여기 설명해야함
        //BlockManager.Instance.MakeLastWord(firstWord);

        hasSuggestion = false;
    }

    //끝말잇기 로직이 들어가는 부분
    private void OnClickConfirmButton()
    {
        //확인 버튼 누르면 확인하는 순서
        //1. 전체 단어에 있는 단어인지 확인
        //2. 끝말잇기가 되는지 확인
        string inputText;
        if (_currentSuggetion == null)
        {
            inputText = input.text;
        }
        else
        {
            inputText = _currentSuggetion;
        }

        //사용한 단어 예외처리
        if (WordStorageManager.Instance.wordStorage.UsedWord.Contains(inputText))
        {
            BlockManager.Instance.SetPrefabTextRed();
            Debug.LogWarning($"이미 사용한 단어입니다 : {inputText}");
            input.text = "";
            input.ActivateInputField();
            explanationText.text = "이미 사용한 단어입니다.";
            return;
        }

        //사용하지 않은 단어일 경우
        else
        {
            //전체 리스트에 입력한게 있을때
            if (IsInputWordInEveryList(inputText))
            {
                //전체 단어에도 있으면서 끝말잇기도 되는 경우
                if (IsWordChainTrue(inputText))
                {
                    //ShowMean(inputText);
                    //BlockManager.Instance.ConfirmBlock();
                    //BlockManager.Instance.MakeLastWord(inputText);
                }
                //전체 단어에는 있지만 끝말잇기는 되지 않는 경우
                else
                {
                    print("끝말잇기가 안됩니다");
                    BlockManager.Instance.SetPrefabTextRed();
                    //suggestionText.text = "";
                }
            }
            else
            {
                BlockManager.Instance.SetPrefabTextRed();
                explanationText.text = "없는 단어입니다.";
            }


            input.text = "";
            input.ActivateInputField();
        }
    }

    private void OnClickSuggestionButton()
    {
        isOn = !isOn;
        print($"현재 불값 : {isOn}");
    }

    private bool IsInputWordInEveryList(string inputText)
    {
        //두음법칙이 적용된다면
        //if (CheckRuleOfHeading(inputText) == true)
        //{

        //}

        if (WordStorageManager.Instance.wordStorage.EveryWordDict.ContainsKey(inputText))
        {
            return true;
        }
        else
        {
            return false;
        }
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
        char beforeLastWord = wordReplayManager.PreWord[wordReplayManager.PreWord.Length - 1];
        char ruled = beforeLastWord;
        bool hasRule = CheckRuleOfHeading(beforeLastWord);

        //이전 글자의 끝이 두음법칙을 만족한다면 
        if (hasRule)
        {
            //두음법칙이 적용된 후의 문자
            ruled = GetRuledChar(beforeLastWord);
            print("두음 법칙 적용 됨");
        }

        print($"이전 단어 끝글자 : {beforeLastWord}");

        //현재 글자의 앞글자
        char inputFirstWord = input[0];
        print($"현재 단어 앞글자 : {inputFirstWord}");

        ////현재 글자의 앞글자 분해
        //string disassemble = inputFirstWord.ToString().Normalize(NormalizationForm.FormD);
        //char vowels = disassemble[0];
        //print(vowels);

        if (beforeLastWord == inputFirstWord || (hasRule && ruled == inputFirstWord))
        {
            print("끝말잇기 성공");
            wordReplayManager.HandleWordSubmission(input, false);
            _currentSuggetion = null;
            suggestionList.Clear();
            //wordReplayManager.PreWord = input;
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
        this.explanationText.text = WordStorageManager.Instance.wordStorage.EveryWordDict[inputText];
    }

    //input안에 값이 바뀔때 마다 실행
    private void OnInputChanged(string input)
    {
        //버퍼에 아무것도 없으면
        if (string.IsNullOrEmpty(input))
        {
            //비우기
            return;
        }
    }

    private void OnSubmit(string input)
    {
        //버퍼 뒤로 이동
        this.input.MoveTextEnd(false);

    }

    private void OnClickTab()
    {
        if (suggestionList.Count > 0)
        {
            //_currentSuggestionIndex = (_currentSuggestionIndex + 1) % suggestionList.Count;
            //suggestionText.text = suggestionList[_currentSuggestionIndex];
            //print($"{_currentSuggestionIndex}번째 추천 단어 : {suggestionText.text}");
            //SetCurrentSuggestion();
            SetCurrentSuggestion();
            BlockManager.Instance.MakeSuggestionBlock(wordReplayManager.PreWord, _typingWord, _currentSuggetion);
            AudioManager.Instance.PlaySfx(Sfx.TapButton);
        }
    }

    //매치하는 단어가 있는지 확인
    private bool HasMatchWord(string composition)
    {
        if (composition == "")
        {
            _currentSuggetion = null;
            suggestionList.Clear();
            return false;
        }

        if (wordReplayManager.PreWord[wordReplayManager.PreWord.Length - 1] != composition[0])
        {
            return false;
        }


        _currentSuggetion = null;

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

        List<string> temp = suggestionList.ToList();

        foreach (var word in suggestionList)
        {
            if (WordStorageManager.Instance.wordStorage.UsedWord.Contains(word))
            {
                temp.Remove(word);
            }
        }

        suggestionList = temp.ToList();

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
    }

    private static int _index = 0;

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
    }

    //입력한 단어가 두음법칙을 만족하는지 확인
    private bool CheckRuleOfHeading(char word)
    {
        if (HEEJAEGameManager.Instance._ruleOfHeading.ContainsKey(word))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    //입력한 글자에 두음법칙을 적용한 값을 리턴
    private char GetRuledChar(char word)
    {
        char result = HEEJAEGameManager.Instance._ruleOfHeading[word];
        return result;
    }
}
