using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterEntry
{
    public SO_Monster monsterDatas;
    public MonsterFoodTastes foodTastes;
    public MonsterActivityTastes activityTastes;
    public MonsterPlacementTastes placementTastes;
    public MonsterNerighbourTastes nerighbourTastes;
}

[Serializable]
public class MonsterFoodTastes
{
    public List<FoodTypeC> foodLikes;
    public List<FoodTypeC> foodDislikes;
}

[Serializable]
public class MonsterPlacementTastes
{
    public List<Placement> placementLikes;
    public List<Placement> placementDislikes;
}

[Serializable]
public class MonsterActivityTastes
{
    public List<Activity> activityLikes;
}

[Serializable]
public class MonsterNerighbourTastes
{
    public List<SO_Monster> neighbourLikes;
    public List<SO_Monster> neighbourDislikes;
}

[CreateAssetMenu(fileName = "New Bestiary", menuName = "MonsterPalace/Bestiary")]
public class SO_Bestiary : ScriptableObject
{
    public List<MonsterEntry> monsterEntries;
}

