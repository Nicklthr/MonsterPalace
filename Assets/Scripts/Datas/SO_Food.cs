using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "MonsterPalace/Food")]
public class SO_Food : ScriptableObject
{
    public string foodName;
    public Sprite sprite;
    public string description;

    public FoodType[] typeList;

    public bool isUnlocked = false;


}
