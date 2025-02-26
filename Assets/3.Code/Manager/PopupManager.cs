using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupManager : Singleton<PopupManager>
{
    [Header("게임 시작 시 Popup 오브젝트가 켜져있어야 찾을 수 있음")]
    private List<Popup> _popups = new List<Popup>();
    private Stack<Popup> _openPopups = new Stack<Popup>();


    private void Start()
    {
        Init();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Init()
    {
        _popups.Clear();

        Popup[] foundedPopups = GameObject.FindObjectsOfType<Popup>();

        foreach (Popup popup in foundedPopups)
        {
            _popups.Add(popup);
            print($"팝업이 담김 {popup.name}");
        }

        foreach (Popup popup in _popups)
        {
            popup.gameObject.SetActive(false);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init();
    }

    public T PopupOpen<T>() where T : Popup
    {
        T openPopup = _popups.Find((popup) => popup is T) as T;

        if (openPopup != null && !_openPopups.Contains(openPopup))
        {
            openPopup.transform.SetAsLastSibling();
            openPopup.gameObject.SetActive(true);
            _openPopups.Push(openPopup);
        }

        return openPopup;
    }

    public void PopupClose()
    {
        if (_openPopups.Count > 0)
        {
            Popup targetPopup = _openPopups.Pop();
            targetPopup.gameObject.SetActive(false);
        }
    }

    public void PopupCloseAll()
    {
        while (_openPopups.Count > 0)
        {
            PopupClose();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
