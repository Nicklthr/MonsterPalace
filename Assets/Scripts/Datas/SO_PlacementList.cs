using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placement List", menuName = "MonsterPalace/Bestiary/PlacementList")]
public class SO_PlacementList : ScriptableObject
{
    List<Placement> placementList;
}

[Serializable]
public class Placement
{
    public RoomPlacement type;
    public Sprite sprite;
}