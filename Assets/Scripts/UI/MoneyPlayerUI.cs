using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyPlayerUI : MonoBehaviour
{
    [SerializeField] private ArgentSO _argentSO;
    [SerializeField] private TextMeshProUGUI _argentText;

    void Update()
    {
        _argentText.text = _argentSO.playerMoney + " $";
    }
}
