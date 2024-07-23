using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _customerName;
    [SerializeField] private Image _customerImage;

    public void SetCustomerData(string name, Sprite image)
    {
        _customerName.text = name;
        _customerImage.sprite = image;
    }
}
