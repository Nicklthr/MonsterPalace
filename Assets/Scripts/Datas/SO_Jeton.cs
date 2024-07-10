using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerCoin", menuName = "MonsterPalace/Jeton")]
public class JetonSO : ScriptableObject
{
    public float playerCoin = 0f;

    public float payedAmount;
    public float gainedAmount;

    public void CoinPay()
    {
        playerCoin -= payedAmount;
    }

    public void CoinGain()
    {
        playerCoin += gainedAmount;
    }
}
