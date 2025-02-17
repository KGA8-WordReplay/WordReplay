using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = typeof(T).Name;
                    _instance = go.AddComponent<T>();
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
