using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private Dictionary<string, string> dic = new Dictionary<string, string>();

    private bool isDone = false;

    private void Awake()
    {
        confirmButton.onClick.AddListener(OnClickConfirmButton);
    }

    private void Start()
    {
        //List<Dictionary<string, object>> csvData = CSVReader.Read("korean_words");

        //foreach (var row in csvData)
        //{
        //    string word = row["단어"].ToString();
        //    //string mean = row["뜻"].ToString();

        //    wordList.Add(word);

        //    //meanList.Add(mean);

        //    //dic[word] = mean;
        //}
        //print(wordList[110000]);
        //isDone = true;
        StartCoroutine(LoadData());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //confirmButton.onClick.Invoke();
            print(wordList[10000]);
            print(meanList[10000]);
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
        csvData = HEEJAECSVTEST.Read("korean_words");
        yield return new WaitForSeconds(20f);

        foreach (var row in csvData)
        {
            string word = row["단어"].ToString();
            string mean = row["뜻"].ToString();

            wordList.Add(word);
            meanList.Add(mean);

            dic[word] = mean;
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
        string mean = dic[inputText];
        this.mean.text = mean;
    }

    private void StartWordChain()
    {

    }
}
