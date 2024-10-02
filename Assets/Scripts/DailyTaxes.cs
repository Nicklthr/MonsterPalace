using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DailyTaxes : MonoBehaviour
{
    private DayNightCycle _cycle;
    private MoneyManager _moneyManager;
    private HotelController _hotel;

    [SerializeField]
    private int taxeHour = 8;

    [SerializeField] bool useTaxeDay = false;
    [SerializeField] private int taxeDay = 1;
    [SerializeField] private int taxeDayAlert = 7;

    private bool taxed = false;

    /*[Header("Room Number")]
    [Tooltip("Nombre de pièces de chaque type présentes dans l'hôtel")]
    public int basicRoomNumber;
    public int specialRoomNumber;
    public int activityRoomNumber;

    [Header("Room Taxes")]
    [Tooltip("Coût quotidien pour chaque type de pièces")]
    public float basicRoomTaxes = 7f;
    public float specialRoomTaxes = 9f;
    public float activityRoomTaxes = 15f;*/

    public float taxesPrice = 100f;
    public float increaseTaxesAmount = 50f;
    public bool increasedTaxes = false;
    public string taxesTxt = "";

    public UnityEvent OnTaxesPaid = new UnityEvent();

    private void Start()
    {
        _cycle = FindObjectOfType<DayNightCycle>();
        _moneyManager = FindObjectOfType<MoneyManager>();
        _hotel = FindObjectOfType<HotelController>();
    }

    void Update()
    {

        taxesTxt = "Next cost of taxes : " + taxesPrice;

        if (useTaxeDay)
        {
            //Debug.Log(_cycle.dayOfTheWeek + " " + taxeDay + " " + taxed);
            if (_cycle.dayOfTheWeek == taxeDay && !taxed)
            {
                Taxes();
            }

            if (_cycle.dayOfTheWeek != taxeDay)
            {
                taxed = false;
                increasedTaxes = false;
            }
        }
        else
        {
            if (_cycle.currentHour == taxeHour && !taxed)
            {
                Taxes();
            }

            if (_cycle.currentHour == taxeHour + 1)
            {
                taxed = false;
                increasedTaxes = false;
            }
        }
    }

    public void Taxes()
    {
        _moneyManager.PayTaxe(taxesPrice);

        IncreaseTaxes();

        //_moneyManager.PayTaxe(basicRoomTaxes * FindCountRoomByType(RoomType.BASE) * 7);
        //_moneyManager.PayTaxe(specialRoomTaxes * FindCountRoomByType(RoomType.BEDROOM) * 7);
        //_moneyManager.PayTaxe(activityRoomTaxes * FindCountRoomByType(RoomType.ACTIVITY) * 7);

        taxed = true;

        OnTaxesPaid.Invoke();
    }

    public void IncreaseTaxes()
    {
        taxesPrice += increaseTaxesAmount;

        increasedTaxes = true;
    }

    /*public int FindCountRoomByType(RoomType type)
    {
        int count = _hotel._hotel.rooms.FindAll(room => room.type == type).Count;

        if ( count == 0 )
        {
            //Debug.LogWarning("Aucune pièce de type " + type + " n'a été trouvée.");
            return 0;
        }
        else
        {
            return count;
        }
    }*/
}
