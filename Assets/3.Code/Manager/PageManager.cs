using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] private GameObject _pagesGO;
    private List<Page> _pages = new List<Page>();


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

    private void Reset()
    {
        Find
    }
}
