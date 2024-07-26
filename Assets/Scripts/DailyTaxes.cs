using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyTaxes : MonoBehaviour
{
    private DayNightCycle _cycle;
    private MoneyManager _argent;
    private HotelController _hotel;

    [SerializeField]
    private int taxeHour = 8;

    private bool taxed = false;

    [Header("Room Number")]
    [Tooltip("Nombre de pièces de chaque type présentes dans l'hôtel")]
    public int basicRoomNumber;
    public int specialRoomNumber;
    public int activityRoomNumber;

    [Header("Room Taxes")]
    [Tooltip("Coût quotidien pour chaque type de pièces")]
    public float basicRoomTaxes = 12f;
    public float specialRoomTaxes = 16f;
    public float activityRoomTaxes = 22f;

    private void Start()
    {
        _cycle = FindObjectOfType<DayNightCycle>();
        _argent = FindObjectOfType<MoneyManager>();
        _hotel = FindObjectOfType<HotelController>();
    }

    void Update()
    {
        if ( _cycle.currentHour == taxeHour && !taxed )
        {
            Taxes();
        }

        if ( _cycle.currentHour == taxeHour + 1 )
        {
            taxed = false;
        }
    }

    public void Taxes()
    {

        _argent.PayTaxe(basicRoomTaxes * FindCountRoomByType(RoomType.BASE));
        _argent.PayTaxe(specialRoomTaxes * FindCountRoomByType(RoomType.BEDROOM));
        _argent.PayTaxe(activityRoomTaxes * FindCountRoomByType(RoomType.ACTIVITY));

        taxed = true;

        Debug.Log("Payement quotidien effectué!");
    }

    public int FindCountRoomByType(RoomType type)
    {
        int count = _hotel._hotel.rooms.FindAll(room => room.type == type).Count;

        if ( count == 0 )
        {
            Debug.LogWarning("Aucune pièce de type " + type + " n'a été trouvée.");
            return 0;
        }
        else
        {
            return count;
        }
    }
}
