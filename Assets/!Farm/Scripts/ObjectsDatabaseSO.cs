using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectsDatabaseSO", menuName = "GameCore/ObjectsDatabaseSO")]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> data;
}

[Serializable]
public record ObjectData
{
    public string name;
    public int id;
    public Vector2Int size = Vector2Int.one;
    public GameObject prefab;
    public EPlaceableType placeableType;
}