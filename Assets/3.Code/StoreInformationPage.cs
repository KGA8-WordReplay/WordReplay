using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreInformationPage : MonoBehaviour
{
    [SerializeField] private Button _backButton;

    private void Start()
    {
        _backButton.onClick.AddListener(BackButtonClick);
    }

    private void BackButtonClick()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _backButton?.onClick.RemoveListener(BackButtonClick);
    }
}
