using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomBtnUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private TextMeshProUGUI _price;

    public void SetName(string name)
    {
        _name.text = name;
    }

    public void SetPrice(int price)
    {
        _price.text = price.ToString();
    }
}
