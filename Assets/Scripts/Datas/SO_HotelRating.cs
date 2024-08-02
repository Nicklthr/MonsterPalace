using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHotelRating", menuName = "MonsterPalace/HotelRating")]

public class SO_HotelRating : ScriptableObject
{
    public int currentStartRating = 0;
    public int intialStartRating = 0;
    public float satisfactionQuantity = 0;
    public float startSatisfactionQuantity = 0;

    public void InitializeRateing()
    {
        currentStartRating = intialStartRating;
        satisfactionQuantity = startSatisfactionQuantity;
    }
}
