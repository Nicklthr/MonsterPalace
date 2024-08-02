using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour
{
    public SO_HotelRating hotelRating;
    [SerializeField] private ArgentSO _argentSO;
    public float playerMoney => _argentSO.playerMoney;
    public float intialMoney = 1000f;
    public float alertThreshold = 200f;

    private float rentalPrice = 0f;
    public float[] dailyCosts = { 55f, 65f, 80f, 110f, 155f };

    public UnityEvent OnRunOutMoney = new UnityEvent();
    public UnityEvent OnPayementDone = new UnityEvent();
    public event Action OnPayement;
    public event Action OnMoneyChange;

    private HotelController _hotelController;

    private void Start()
    {
        _hotelController = FindObjectOfType<HotelController>();
        InitializeMoney();

    }


    private void Update()
    {
        if ( _argentSO.playerMoney <= 0 )
        {
            OnRunOutMoney.Invoke();
        }
    }

    private void InitializeMoney()
    {
        _argentSO.playerMoney = intialMoney;
        OnMoneyChange?.Invoke();
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

    public void PayRoom( float price )
    {
        if ( _argentSO.playerMoney < price )
        {
            return;
        }

        _argentSO.playerMoney -= price;
        OnMoneyChange?.Invoke();
    }

    public void PayTaxe( float price )
    {
        if ( _argentSO.playerMoney < price )
        {
            _argentSO.playerMoney = 0;
        }

        if (price <= 0)
        {
            return;
        }

        _argentSO.playerMoney -= price;
        OnMoneyChange?.Invoke();
    }

    public void AddMoney( float price )
    {
        _argentSO.playerMoney += price;
        OnMoneyChange?.Invoke();
    }
}
