using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class InformationPage : Page
{
    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;

    public List<GameObject> informations = new List<GameObject>();
    private Stack<GameObject> informationStack = new Stack<GameObject>();

    private void Start()
    {
        _prevButton.onClick.AddListener(PrevButtonClick);
        _nextButton.onClick.AddListener(NextButtonClick);

        foreach (GameObject obj in informations)
        {
            obj.SetActive(false);
        }

        informations[0].SetActive(true);
    }

    private void PrevButtonClick()
    {

    }

    private void NextButtonClick()
    {

    }

    private void OnDestroy()
    {
        _prevButton.onClick.RemoveListener(PrevButtonClick);
        _nextButton.onClick.RemoveListener(NextButtonClick);
    }
}
