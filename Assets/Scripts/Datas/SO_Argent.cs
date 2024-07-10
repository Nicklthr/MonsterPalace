using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerMoney", menuName = "MonsterPalace/Argent")]
public class ArgentSO : ScriptableObject
{
    public float playerMoney = 0f;

    public float payedAmount;
    public float gainedAmount;

    public void MoneyPay()
    {
        playerMoney -= payedAmount;
    }

    public void MoneyGain()
    {
        playerMoney += gainedAmount;
    }
}
