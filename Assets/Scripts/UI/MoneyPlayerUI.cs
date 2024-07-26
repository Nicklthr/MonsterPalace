using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyPlayerUI : MonoBehaviour
{
    [SerializeField] private ArgentSO _argentSO;
    [SerializeField] private TextMeshProUGUI _argentText;
    [SerializeField] private MoneyManager _moneyManager;
    [SerializeField] private PlacementSystem _placementSystem;

    private void Start()
    {
        _moneyManager = FindObjectOfType<MoneyManager>();
        _placementSystem = FindObjectOfType<PlacementSystem>();

        UpdateUI();

        _moneyManager.OnPayement += UpdateUI;
        _moneyManager.OnMoneyChange += UpdateUI;

        _placementSystem.OnRoomPlaced += UpdateUI;
        _placementSystem.OnStairPlaced += UpdateUI;
    }

    private void UpdateUI()
    {
        _argentText.text = _argentSO.playerMoney + " $";
    }
}
