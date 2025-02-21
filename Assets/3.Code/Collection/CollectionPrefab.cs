using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPrefab : MonoBehaviour
{
    [SerializeField] private CollectionData collectionData;
    [SerializeField] private Image lockedImage;
    [SerializeField] private Image collectionImage;
    [SerializeField] private TextMeshProUGUI collectionName;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private bool isActive;
    [SerializeField] private Button activeButton;


    private void Awake()
    {
        activeButton.onClick.AddListener(OnClickButton);
    }

    private void Start()
    {
        Init();

        //활성화true -> Active(false)
        if (isActive == true)
        {
            lockedImage.gameObject.SetActive(false);
        }
        else
        {
            lockedImage.gameObject.SetActive(true);
        }
    }
    
    private void Init()
    {
        collectionImage.sprite = collectionData.image;
        collectionName.text = collectionData.textName;
        //gold.text = collectionData.gold.ToString();
    }

    private void HandleLocked()
    {

    }

    private GoldPopup _opendPopup;
    private void OnClickButton()
    {
        //isActive = !isActive;

        if (isActive == true)
        {
            //lockedImage.gameObject.SetActive(false);
        }
        else
        {
            _opendPopup = PopupManager.Instance.PopupOpen<GoldPopup>();
            _opendPopup.SetPopup(collectionData, "구매하시겠습니까?", OnPurchaseResult);
            //lockedImage.gameObject.SetActive(true);
        }
    }

    private void OnPurchaseResult(bool isSuccess)
    {
        //구매 성공하면
        if (isSuccess == true)
        {
            InformationPopup popup = PopupManager.Instance.PopupOpen<InformationPopup>();

            popup.SetPopup(collectionData, "사전 획득!");

            CollectionDataManager.Instance.PurchaseWordDict(collectionName.text);
        }
        else
        {
            _opendPopup.NotEnoughMoney();
        }
    }
}
