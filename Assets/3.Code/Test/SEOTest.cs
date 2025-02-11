using Seo;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SEOTest : MonoBehaviour
{
    public TextMeshProUGUI wordText;
    public TextMeshProUGUI explanationText;
    public int _count;

    private List<Dictionary<string, object>> _everyWord = new List<Dictionary<string, object>>();

    private void Awake()
    {
        _everyWord = CSVReader.Read("EveryWord");
        print(_everyWord.Count);
    }

    public void NextButton()
    {
        if (_count >= _everyWord.Count) _count = 0;

        string word = _everyWord[_count]["어휘"].ToString();
        string explanation = _everyWord[_count]["뜻풀이"].ToString();
        wordText.text = word;
        explanationText.text = explanation;

        _count++;
    }
}
