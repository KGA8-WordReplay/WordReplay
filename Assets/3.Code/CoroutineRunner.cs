using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : Singleton<CoroutineRunner>
{
    public void StartRoutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}
