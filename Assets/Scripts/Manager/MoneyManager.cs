using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour
{
    public SO_HotelRating hotelRating;
    [SerializeField] private ArgentSO _argentSO;

    private float rentalPrice = 0f;
    public float[] dailyCosts = { 55f, 65f, 80f, 110f, 155f };

    public UnityEvent OnRunOutMoney = new UnityEvent();
    public UnityEvent OnPayementDone = new UnityEvent();
    public event Action OnPayement;
    public event Action OnMoneyChange;

    public float DailyHotelCostsBase = 10f;
    private float _dailyHotelCosts;

    private DayNightCycle _dayNightCycle;
    private HotelController _hotelController;

    private void Start()
    {
        _dayNightCycle = FindObjectOfType<DayNightCycle>();
        _hotelController = FindObjectOfType<HotelController>();

        _dayNightCycle.OnNextDayAction += DailyHotelCosts;
    }


    private void Update()
    {
        if ( _argentSO.playerMoney <= 0 )
        {
            OnRunOutMoney.Invoke();
        }
    }

    public void Payment(int stayDuration)
    {
        switch ( hotelRating.currentStartRating )
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

        _argentSO.playerMoney += rentalPrice;

        OnPayement?.Invoke();
        OnPayementDone.Invoke();
    }

    public void DailyHotelCosts()
    {
        _dailyHotelCosts = DailyHotelCostsBase * _hotelController._hotel.rooms.Count;

        if ( _argentSO.playerMoney < _dailyHotelCosts )
        {
            _argentSO.playerMoney = 0;
            return;
        }

        _argentSO.playerMoney -= _dailyHotelCosts;
    }
}
