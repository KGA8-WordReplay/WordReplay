using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public TextMeshProUGUI word;

    public void SetWord(char word, bool isStartOrEnd)
    {
        //두음법칙 성립 안하면 그냥 출력
        if (isStartOrEnd == false || CheckRuleOfHeading(word) == false)
        {
            this.word.text = word.ToString();
        }

        else
        {
            SetRuleOfHeading(word);
        }
    }

    private void SetRuleOfHeading(char word)
    {
        if (word == 0)
        {
            return;
        }
        
        char ruleOfHeading = HEEJAEGameManager.Instance._ruleOfHeading[word];
        string result = "";
        result = $"{word.ToString()}({ruleOfHeading.ToString()})";

        this.word.text = result;
        this.word.fontSize = 0.35f;
    }

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
}
