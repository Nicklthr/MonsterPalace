using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevManager : MonoBehaviour
{
    [SerializeField] private ArgentSO argentSO;
    [SerializeField] private SO_HotelRating hotelRating;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            argentSO.playerMoney += 1000;
        }

        if (Input.GetKey(KeyCode.RightAlt))
        {
            hotelRating.currentStartRating = 4;
        }
    }
}
