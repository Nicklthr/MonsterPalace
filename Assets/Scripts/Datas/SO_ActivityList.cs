using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Activity List", menuName = "MonsterPalace/Bestiary/ActivityList")]
public class SO_ActivityList : ScriptableObject
{
    public List<Activity> activityList;
}

[Serializable]
public class Activity
{
    public ActivityType type;
    public Sprite sprite;
}
