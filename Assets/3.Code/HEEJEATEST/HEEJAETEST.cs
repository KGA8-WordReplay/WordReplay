using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class HEEJAETEST : MonoBehaviour
{
    public TMP_InputField input;
    public TextMeshProUGUI word;
    public TextMeshProUGUI mean;
    public Button confirmButton;

    public List<string> wordList = new List<string>();
    public List<string> meanList = new List<string>();
    private Dictionary<string, string> _dic = new Dictionary<string, string>();
    private List<Dictionary<string, object>> _everyWord = new List<Dictionary<string, object>>();
    public int _count;

    private bool isDone = false;

    private void Awake()
    {
        //confirmButton.onClick.AddListener(OnClickConfirmButton);
    }

    private void Start()
    {
        _everyWord = HEEJAECSVTEST.Read("EveryWord");
        //List<Dictionary<string, object>> csvData = HEEJAECSVTEST.Read("EveryWord");

        //foreach (var row in csvData)
        //{
        //    string word = row["어휘"].ToString();
        //    string mean = row["뜻풀이"].ToString();

        //    wordList.Add(word);
        //    meanList.Add(mean);

        //    _dic[word] = mean;
        //}

        //isDone = true;
        //StartCoroutine(LoadData());
    }
    public void NextButton()
    {
        if (_count >= _everyWord.Count) _count = 0;

        string word = _everyWord[_count]["어휘"].ToString();
        string explanation = _everyWord[_count]["뜻풀이"].ToString();
        this.word.text = word;
        mean.text = explanation;

        _count++;
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

    private IEnumerator LoadData()
    {
        isDone = false;
        confirmButton.interactable = false;

        List<Dictionary<string, object>> csvData = null;

        //yield return new WaitUntil(() =>
        //{
        //    csvData = CSVReader.Read("korean_words");
        //    return csvData != null && csvData.Count > 0;
        //});
        csvData = HEEJAECSVTEST.Read("EveryWord");
        yield return new WaitForSeconds(20f);

        foreach (var row in csvData)
        {
            string word = row["어휘"].ToString();
            string mean = row["뜻풀이"].ToString();

            wordList.Add(word);
            meanList.Add(mean);

            _dic[word] = mean;
        }

        isDone = true;
        confirmButton.interactable = true;
    }

    private bool CheckInputWordInList(string inputText)
    {
        for (int i = 0; i < wordList.Count; i++)
        {
            if (inputText != wordList[i])
            {
                continue;
            }
            if (inputText == wordList[i])
            {
                word.text = inputText;
                return true;
            }
        }
        return false;
    }

    private void ShowMean(string inputText)
    {
        string mean = _dic[inputText];
        this.mean.text = mean;
    }

    private void StartWordChain()
    {

    }
}
