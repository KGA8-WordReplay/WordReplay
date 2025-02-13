using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordReplayMainUI : MonoBehaviour
{
    public TextMeshProUGUI explanationText;
    public TextMeshProUGUI outputText;
    public TMP_InputField inputText;
    public Button confirmButton;

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
        outputText.text = output;
        explanationText.text = explanation;
    }
}
