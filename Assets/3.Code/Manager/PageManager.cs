using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PageManager : Singleton<PageManager>
{
    private List<Page> _pages = new List<Page>();


    private void Start()
    {
        Init();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init();
    }

    public T OpenPage<T>() where T : Page
    {
        T result = null;

        foreach (Page page in _pages)
        {
            bool isActive = page is T;
            page.gameObject.SetActive(isActive);
            if (isActive) result = page as T;
        }

        return result;
    }

    private void Init()
    {
        _pages.Clear();

        //GameObject pagesGO = GameObject.Find("Pages");


        Page[] pages = FindObjectsOfType<Page>();

            foreach (Page page in pages)
            {
                _pages.Add(page);
                page.gameObject.SetActive(page.isDefault);
                print(page.name + "이 담김");
            }
        print("Init이 작동함");
    }
}
