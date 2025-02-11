using Seo;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SEOTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _wordText;
    [SerializeField] private TextMeshProUGUI _explanationText;

    private List<Dictionary<string, object>> _everyWord = new List<Dictionary<string, object>>();
    public int _count;

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
        _wordText.text = word;
        _explanationText.text = explanation;

        _count++;
    }
}
