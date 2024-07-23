using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu(fileName = "New Monster", menuName = "MonsterPalace/Monster")]
public class SO_Monster : ScriptableObject
{

    public MonsterType monsterType;
    public string speciesDescription;

    [Range(0, 23)]
    public int eatingHourMin;
    [Range(0, 23)]
    public int eatingHourMax;

    [Range(0, 23)]
    public int activityHourMin;
    [Range(0, 23)]
    public int activityHourMax;

    public MonsterType[] neighboorLike;
    public MonsterType[] neighboorDislike;

    public RoomPlacement[] roomPlacementsLike;
    public RoomPlacement[] roomPlacementsDislike;

    public FoodType[] foodLike;
    public FoodType[] foodDislike;

    public ActivityType[] activityLike;

    public float patienceMax;

    public SO_NameList monsterNameList;

    public Sprite monsterSprite;

}


