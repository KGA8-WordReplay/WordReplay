using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPopup : Popup
{
    [SerializeField] private Image collectionImage;
    [SerializeField] private TextMeshProUGUI collectionName;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button closeButton;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(OnClickCloseButton);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveAllListeners();
    }

    public void SetPopup(CollectionData collection, string text)
    {
        collectionImage.sprite = collection.image;
        this.collectionName.text = collection.textName;
        this.text.text = text;
    }

    private void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySfx(Sfx.Button);
        PopupManager.Instance.PopupClose();
    }
}
