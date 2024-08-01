using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

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

    private void Update()
    {
        if ( _argentSO.playerMoney < _moneyManager.alertThreshold )
        {
              gameObject.GetComponent<MMF_Player>().PlayFeedbacks();
        }
        else
        {
            gameObject.GetComponent<MMF_Player>().StopFeedbacks();
            _argentText.color = Color.white;
        }
    }

    private void UpdateUI()
    {
        int money = (int)_argentSO.playerMoney;

        NumericTextAnimator.Instance.AnimateTextTo(_argentText, money, 1f, "$");
    }
}
