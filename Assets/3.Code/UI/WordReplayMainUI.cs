using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordReplayMainUI : MonoBehaviour
{
    public TextMeshProUGUI explanationText;
    public TextMeshProUGUI outputText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI maxScoreText;
    public TMP_InputField inputText;
    public Button confirmButton;
    public Button quitButton;

    public Button autoButton;
    public bool onAuto = false;
    private WordReplayManager _wordReplayManager;

    private void Awake()
    {
        _wordReplayManager = FindObjectOfType<WordReplayManager>();
        autoButton.onClick.AddListener(AutoButtonClick);
    }
    private void OnDestroy()
    {
        autoButton.onClick.RemoveListener(AutoButtonClick);
    }

    private void AutoButtonClick()
    {
        onAuto = !onAuto;
        _wordReplayManager.AutoMode(onAuto);
    }

    public void AutoButtonColor()
    {
        autoButton.GetComponent<Image>().color = onAuto ? Color.green : Color.white;
    }

    public void UpdateWordDisplay(string output, string explanation)
    {
        print($"들어간 단어 : {output}");
        print($"이전 단어 : {_wordReplayManager.PreWord}");
        //outputText.text = output;
        BlockManager.Instance.MakeBlock(_wordReplayManager.PreWord, output);
        explanationText.text = explanation;
    }

    public void AddScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetMaxScore(int score)
    {
        maxScoreText.text = score.ToString();
    }
}
