using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour
{
    public SO_HotelRating hotelRating;

    public float rentalPrice = 0f;
    public float[] dailyCosts = { 55f, 65f, 80f, 110f, 155f };

    public UnityEvent OnPayementDone = new UnityEvent();
    public event Action OnPayement;

    public void Payment(int stayDuration)
    {
        switch (hotelRating.currentStartRating)
        {
            case 0:
                rentalPrice = dailyCosts[0] * stayDuration;
                break;
            case 1:
                rentalPrice = dailyCosts[1] * stayDuration;
                break;
            case 2:
                rentalPrice = dailyCosts[2] * stayDuration;
                break;
            case 3:
                rentalPrice = dailyCosts[3] * stayDuration;
                break;
            case 4:
                rentalPrice = dailyCosts[4] * stayDuration;
                break;
            default:
                break;
        }

        OnPayement?.Invoke();
        OnPayementDone.Invoke();
    }
}
