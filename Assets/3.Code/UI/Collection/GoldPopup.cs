using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldPopup : Popup
{
    [SerializeField] private Image collectionImage;
    [SerializeField] private TextMeshProUGUI collectionName;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button goldButton;
    [SerializeField] private Button closeButton;

    private Action<bool> _callback;

    private CollectionData _collectionData;

    private void OnEnable()
    {
        goldButton.image.color = Color.white;
        goldButton.onClick.AddListener(OnClickGoldButton);
        closeButton.onClick.AddListener(OnClickCloseButton);
    }

    private void OnDisable()
    {
        goldButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
    }

    public void SetPopup(CollectionData collection, string text, Action<bool> callback)
    {
        if (_collectionData != null)
        {
            _collectionData = null;
        }

        _collectionData = collection;
        _callback = callback;

        collectionImage.sprite = _collectionData.image;
        this.collectionName.text = _collectionData.textName;
        goldText.text = _collectionData.gold.ToString();
        this.text.text = text;
    }

    private void OnClickGoldButton()
    {
        //골드가 충분히 있다면
        if(TempGoldSingleton.Instance.gold >= _collectionData.gold)
        {
            PopupManager.Instance.PopupClose();
            _callback?.Invoke(true);
        }
        //없다면
        else
        {
            _callback?.Invoke(false);
        }
    }

    private void OnClickCloseButton()
    {
        PopupManager.Instance.PopupClose();
    }

    public void NotEnoughMoney()
    {
        text.text = "골드가 부족합니다.";
        goldButton.image.color = Color.red;
    }
}
