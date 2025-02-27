using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Switch : MonoBehaviour
{
    [SerializeField] private Image on;
    [SerializeField] private Image off;
    [SerializeField] private Button suggestionButton;
    private bool isOn;
    private void Start()
    {
        isOn = HEEJAEGameManager.Instance.isOn;
        suggestionButton.onClick.AddListener(OnClickSuggestionButton);
    }
    private void Update()
    {
        if (isOn == true)
        {
            on.gameObject.SetActive(true);
            off.gameObject.SetActive(false);
        }
        else
        {
            on.gameObject.SetActive(false);
            off.gameObject.SetActive(true);
        }
    }
    private void OnDisable()
    {
        suggestionButton.onClick.RemoveAllListeners();
    }
    private void OnClickSuggestionButton()
    {
        isOn = !isOn;
    }
}