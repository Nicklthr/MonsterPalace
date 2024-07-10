using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyTaxes : MonoBehaviour
{
    public DayNightCycle cycle;
    public ArgentSO argent;

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

    void Update()
    {
        if (cycle.currentHour == taxeHour && !taxed)
        {
            Taxes();
        }

        if (cycle.currentHour == taxeHour + 1)
        {
            taxed = false;
        }
    }

    public void Taxes()
    {
        argent.playerMoney -= basicRoomTaxes * basicRoomNumber;
        argent.playerMoney -= specialRoomTaxes * specialRoomNumber;
        argent.playerMoney -= activityRoomTaxes * activityRoomNumber;
        taxed = true;
        Debug.Log("Payement quotidien effectué!");
        Debug.Log(argent.playerMoney);
    }
}
