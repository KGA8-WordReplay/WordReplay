using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public TextMeshProUGUI word;

    public void SetWord(char word)
    {
        this.word.text = word.ToString();
    }
}
