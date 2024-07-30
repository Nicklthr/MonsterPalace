using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomInformationsPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _maxCapacity;
    [SerializeField] private TextMeshProUGUI _currentUserCount;
    [SerializeField] private TextMeshProUGUI _light;

    public void SetRoomInformations(int maxCapacity, int userCount, string light)
    {
        _maxCapacity.text = "Max users : " + maxCapacity.ToString();
        _currentUserCount.text = "Current users : " + userCount.ToString();
        _light.text = light;
    }
}