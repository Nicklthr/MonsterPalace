using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomInformationsPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _maxCapacity;
    [SerializeField] private TextMeshProUGUI _currentUserCount;

    public void SetRoomInformations(int maxCapacity, int userCount)
    {
        _maxCapacity.text = "Max users : " + maxCapacity.ToString();
        _currentUserCount.text = "Current users : " + userCount.ToString();
    }
}