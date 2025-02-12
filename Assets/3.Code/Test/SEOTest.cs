using Seo;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SEOTest : MonoBehaviour
{
    public TextMeshProUGUI wordText;
    public TextMeshProUGUI explanationText;
    public int count;

    private List<Dictionary<string, object>> _everyWord = new List<Dictionary<string, object>>();

    private void Awake()
    {
        //_everyWord = CSVReader.Read("Word/EveryWord");
        //print(_everyWord.Count);
    }

    public void NextButton()
    {
        if (count >= _everyWord.Count) count = 0;

        string word = _everyWord[count]["어휘"].ToString();
        string explanation = _everyWord[count]["뜻풀이"].ToString();
        wordText.text = word;
        explanationText.text = explanation;

        count++;
    }
}
