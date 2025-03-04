using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.GameSystem.Placement
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] List<GameObject> placedGameObjects = new();

        public int PlaceObject(GameObject prefab, Vector3 position)
        {
            return PlaceObject(prefab, position, out _);
        }
        public int PlaceObject(GameObject prefab, Vector3 position, out GameObject instance)
        {
            GameObject newObject = Instantiate(prefab);
            newObject.transform.position = position;
            placedGameObjects.Add(newObject);
            instance = newObject;
            return placedGameObjects.Count - 1;
        }

        public void RemoveObjectAt(int gameObjectIndex)
        {
            if (placedGameObjects.Count <= gameObjectIndex
                || placedGameObjects[gameObjectIndex] == null)
                return;
            Destroy(placedGameObjects[gameObjectIndex]);
            placedGameObjects[gameObjectIndex] = null;
        }

        public GameObject GetObjectAt(int gameObjectIndex)
        {
            if (placedGameObjects.Count <= gameObjectIndex
                || placedGameObjects[gameObjectIndex] == null)
                return null;
            return placedGameObjects[gameObjectIndex];
        }
    }
}