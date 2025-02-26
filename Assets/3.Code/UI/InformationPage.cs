using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPage : Page
{
    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;

    public List<GameObject> informations = new List<GameObject>();
}
