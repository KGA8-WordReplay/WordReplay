using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CollectionData", menuName = "Scriptable Object/CollectionData")]
public class CollectionData : ScriptableObject
{
    public string textName;
    public Sprite image;
    public int gold;
}
    