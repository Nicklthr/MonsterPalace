using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedRooms = new();

    public void AddRoomAt(Vector3Int gridPosition, Vector2Int roomSize, int ID, int placedRoomIndex)
    {
        List<Vector3Int> positionToOccupy = CalculationPositions(gridPosition, roomSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedRoomIndex);

        foreach (var pos in positionToOccupy)
        {
            if (placedRooms.ContainsKey(pos))
            {
                throw new Exception("Dictionary already contains key");
            }
            placedRooms[pos] = data;
        }
    }

    private List<Vector3Int> CalculationPositions(Vector3Int gridPosition, Vector2Int roomSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < roomSize.x; x++)
        {
            for (int y = 0; y < roomSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceRoomAt(Vector3Int gridPosition, Vector2Int roomSize)
    {
        List<Vector3Int> positionToOccupy = CalculationPositions(gridPosition, roomSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedRooms.ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;

    public int ID { get; private set; }
    public int PlacedRoomIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedRoomIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedRoomIndex = placedRoomIndex;
    }
}
