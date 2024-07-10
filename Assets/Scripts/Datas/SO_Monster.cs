using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Monster", menuName = "MonsterPalace/Monster")]
public class SO_Monster : ScriptableObject
{

    public string speciesName;
    public string speciesDescription;

    [Range(0, 23)]
    public int eatingHourMin;
    [Range(0, 23)]
    public int eatingHourMax;

    [Range(0, 23)]
    public int activityHourMin;
    [Range(0, 23)]
    public int activityHourMax;

    public SO_Monster[] neighboorLike;
    public SO_Monster[] neighboorDislike;

    public RoomPlacement[] roomPlacementsLike;
    public RoomPlacement[] roomPlacementsDislike;

    public FoodType[] foodLike;
    public FoodType[] foodDislike;

    public SO_Activity[] activityLike;

    public float patienceMax;

}


