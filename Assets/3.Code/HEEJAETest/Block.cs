using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public TextMeshProUGUI word;
    public List<Sprite> sprites;
    public static float blockLength;

    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        switch (BlockManager.Instance.stageName)
        {
            case "stage1":
                spriteRenderer.sprite = sprites[0];
                break;
            case "stage2":
                spriteRenderer.sprite = sprites[1];
                break;
            case "stage3":
                spriteRenderer.sprite = sprites[2];
                break;
            case "stage4":
                spriteRenderer.sprite = sprites[3];
                break;
        }

        blockLength = spriteRenderer.bounds.size.x; // 가로 길이 가져오기
    }

    public void SetWord(char word, char ruledWord, bool isStart)
    {
        //두음법칙 성립 안하면 그냥 출력
        if (isStart == false || ruledWord == '\0')
        {
            this.word.text = word.ToString();
        }

        else
        {
            this.word.text = $"{word}({ruledWord})";
            this.word.fontSize = 0.35f;
            this.word.color = Color.black;
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
        this.word.color = Color.black;
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
