using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHotelRating", menuName = "MonsterPalace/HotelRating")]

public class SO_HotelRating : ScriptableObject
{
    public int currentStartRating = 0;
    public int intialStartRating = 2;

    public void InitializeRateing()
    {
        currentStartRating = intialStartRating;
    }
}
