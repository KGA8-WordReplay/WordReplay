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
    [SerializeField] private Image checkImage;
    [SerializeField] private TextMeshProUGUI collectionName;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private Button activeButton;
    public bool isActive;


    private void Awake()
    {
        activeButton.onClick.AddListener(OnClickButton);
    }

    private void Start()
    {
        Init();
        print("CollectionPrefab에서 옴");
        //활성화true -> Active(false)
        if (isActive == true)
        {
            lockedImage.gameObject.SetActive(false);
            checkImage.gameObject.SetActive(true);
        }
        else
        {
            lockedImage.gameObject.SetActive(true);
            checkImage.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (isActive == true)
        {
            lockedImage.gameObject.SetActive(false);
            checkImage.gameObject.SetActive(true);
        }
        else
        {
            lockedImage.gameObject.SetActive(true);
            checkImage.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        //Init();
        print("프리팹 꺼짐");
    }

    private void Init()
    {
        collectionImage.sprite = collectionData.image;
        collectionName.text = collectionData.textName;
        //gold.text = collectionData.gold.ToString();
        string collection = $"『{collectionName.text}』";
        print($"콜렉션 이름 : {collection}");
        //foreach(var temp in CollectionDataManager.Instance.myWordNameList)
        //{
        //    print(collection.Equals(temp));
        //}
        //print(collection.Equals(CollectionDataManager.Instance.myWordNameList.Contains(collection)));

        if (CollectionDataManager.Instance.myWordNameList.Contains(collection))
        {
            print("있는 사전임!");
            isActive = true;
        }
        else
        {
            isActive = false;
            print("여기임");
        }
        print("프리팹 켜짐");

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

            string temp = $"『{collectionName.text}』";
            CollectionDataManager.Instance.PurchaseWordDict(temp);

            isActive = true;
            lockedImage.gameObject.SetActive(false);
            checkImage.gameObject.SetActive(true);
        }
        else
        {
            _opendPopup.NotEnoughMoney();
        }
    }
}
