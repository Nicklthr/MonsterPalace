using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardFoodUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _foodName;
    [SerializeField] private Image _foodImage;


    public void SetFood( SO_Food food )
    {
        _foodName.text = food.foodName;
        //_foodImage.sprite = food.foodImage;
    }
}
