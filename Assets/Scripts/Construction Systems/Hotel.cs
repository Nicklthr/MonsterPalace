using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotel : MonoBehaviour
{
    public int width = 10;
    public int height = 20;
    public int depth = 4;
    public float cellSize = 1f;

    public int money = 1000;

    public SO_RoomType[,,] grid;

    private void Start()
    {
        grid = new SO_RoomType[height, width, depth];
    }
}