using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects;
    Vector2Int gridSize;

    public GridData(Vector2Int gridSize)
    {
        this.gridSize = gridSize;
        placedObjects = new();
    }

    public void AddObject(Vector3Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, id, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell position {pos}");
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        for (int x = 0; x < objectSize.x; x++)
            for (int y = 0; y < objectSize.y; y++)
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));

        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        if (gridPosition.x + objectSize.x > gridSize.x) return false;
        if (gridPosition.z + objectSize.y > gridSize.y) return false;

        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }

    public int GetGameObjectIndex(Vector3Int gridPosition)
    {
        if (!placedObjects.ContainsKey(gridPosition))
            return -1;
        return placedObjects[gridPosition].gameObjectIndex;
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        if (!placedObjects.ContainsKey(gridPosition))
            return;

        var occupied = placedObjects[gridPosition].occupiedPositions;
        foreach (var pos in occupied)
        {
            placedObjects.Remove(pos);
        }
    }
}

public record PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int id { get; private set; }
    public int gameObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        id = iD;
        gameObjectIndex = placedObjectIndex;
    }
}