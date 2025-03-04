using Seo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public bool isPurchased;
    public bool isActive;

    private void Start()
    {
        activeButton.onClick.AddListener(OnClickButton);
        CollectionDataManager.Instance.OnDataLoaded += Init;
        Init();
        print("CollectionPrefab에서 옴");

        UpdateVisuals();
    }

    private void OnEnable()
    {
        UpdateVisuals();
    }

    private void OnDestroy()
    {
        //Init();
        activeButton.onClick.RemoveListener(OnClickButton);
        CollectionDataManager.Instance.OnDataLoaded -= Init;
        print("프리팹 꺼짐");
    }

    private void Init()
    {
        collectionImage.sprite = collectionData.image;
        collectionImage.rectTransform.sizeDelta = new Vector2(140, 120);
        collectionName.text = collectionData.textName;
        //gold.text = collectionData.gold.ToString();
        string collection = $"『{collectionName.text}』";
        //foreach(var temp in CollectionDataManager.Instance.myWordNameList)
        //{
        //    print(collection.Equals(temp));
        //}
        //print(collection.Equals(CollectionDataManager.Instance.myWordNameList.Contains(collection)));

        if (CollectionDataManager.Instance.myWordNameList.Contains(collection))
        {
            isPurchased = true;

            if (CollectionDataManager.Instance.activedNameList.Contains(collection))
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
        }
        else
        {
            isPurchased = false;
            isActive = false;
        }

    }

    private void UpdateVisuals()
    {
        if (isPurchased)
        {
            lockedImage.gameObject.SetActive(false);
            if (isActive)
            {
                checkImage.gameObject.SetActive(true);
            }
            else
            {
                checkImage.gameObject.SetActive(false);
            }
        }
        else
        {
            lockedImage.gameObject.SetActive(true);
            checkImage.gameObject.SetActive(false);
        }
    }

    private GoldPopup _opendPopup;
    private void OnClickButton()
    {
        print("콜렉션 버튼 눌림");
        AudioManager.Instance.PlaySfx(Sfx.Button);

        //구매가 완료될때 누르면 Switch
        if (isPurchased == true)
        {
            //lockedImage.gameObject.SetActive(false);
            Switch();
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

            isPurchased = true;
            lockedImage.gameObject.SetActive(false);
            checkImage.gameObject.SetActive(true);
        }
        else
        {
            _opendPopup.NotEnoughMoney();
        }
    }

    private void Switch()
    {
        isActive = !isActive;
        UpdateCheckImage();

        string temp = $"『{collectionName.text}』";
        if (isActive == true)
        {
            print("isActive가 켜짐");
            List<Dictionary<string, object>> resource = CSVReader.Read($"Word/MyWord/{temp}");
            //그 전환한 파일을 Dict로 전환
            Dictionary<string, string> tempWord = ConvertToStringDictionary(resource, "어휘", "뜻풀이");
            Dictionary<string, string> tempWord2 = ConvertToStringDictionary2(resource, "어휘", "전문 분야");
            //마지막으로 내가 가지고 있는 단어에 추가
            WordStorageManager.Instance.wordStorage.AddMyWordDict(tempWord);
            WordStorageManager.Instance.wordStorage.AddMyWordDict2(tempWord2);

            CollectionDataManager.Instance.AddActiveCollection(temp);
            print($"저장된 파일 이름 : {collectionName}");
        }
        else
        {
            print("isActive가 꺼짐");
            List<Dictionary<string, object>> resource = CSVReader.Read($"Word/MyWord/{temp}");
            //그 전환한 파일을 Dict로 전환
            Dictionary<string, string> tempWord = ConvertToStringDictionary(resource, "어휘", "뜻풀이");
            Dictionary<string, string> tempWord2 = ConvertToStringDictionary2(resource, "어휘", "전문 분야");
            //마지막으로 내가 가지고 있는 단어에 추가
            WordStorageManager.Instance.wordStorage.SubMyWordDict(tempWord);
            WordStorageManager.Instance.wordStorage.SubMyWordDict2(tempWord2);

            CollectionDataManager.Instance.SubActiveCollection(temp);
            print($"저장된 파일 이름 : {collectionName}");
        }
    }

    private Dictionary<string, string> ConvertToStringDictionary(List<Dictionary<string, object>> data, string colName, string colName2)
    {
        Dictionary<string, string> processedData = new Dictionary<string, string>();
        for (int i = 0; i < data.Count; i++)
        {
            string word = data[i][colName].ToString();
            string explanation = data[i][colName2].ToString();

            if (processedData.ContainsKey(word)) continue;
            processedData.Add(word, explanation);
        }

        return processedData;
    }
    private Dictionary<string, string> ConvertToStringDictionary2(List<Dictionary<string, object>> data, string colName, string colName2)
    {
        Dictionary<string, string> processedData = new Dictionary<string, string>();
        for (int i = 0; i < data.Count; i++)
        {
            string word = data[i][colName].ToString();
            string explanation = data[i][colName2].ToString();

            if (processedData.ContainsKey(word)) continue;
            processedData.Add(word, explanation);
        }

        return processedData;
    }

    private void UpdateCheckImage()
    {
        checkImage.gameObject.SetActive(isActive);
    }
}