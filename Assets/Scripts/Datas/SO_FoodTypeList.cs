using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FoodType List", menuName = "MonsterPalace/Bestiary/FoodTypeList")]
public class SO_FoodTypeList : ScriptableObject
{
    List<FoodTypeC> foodTypeList;
}

[Serializable]
public class FoodTypeC
{
    public FoodType type;
    public Sprite sprite;
}
