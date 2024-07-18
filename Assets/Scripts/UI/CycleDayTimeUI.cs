using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CycleDayTimeUI : MonoBehaviour
{
    [SerializeField] private DayNightCycle _dayNightCycle;
    [SerializeField] private TextMeshProUGUI _dayText;
    [SerializeField] private TextMeshProUGUI _timeText;

    private void Start()
    {
        if ( _dayNightCycle == null )
        {
            Debug.LogError( "CycleDayTimeUI: DayNightCycle is not assigned" );
            return;
        }

        if ( _dayText == null )
        {
            Debug.LogError( "CycleDayTimeUI: Day Text is not assigned" );
            return;
        }

        if ( _timeText == null )
        {
            Debug.LogError( "CycleDayTimeUI: Time Text is not assigned" );
            return;
        }
    }

    void Update()
    {
        _dayText.text = "Jour : " + _dayNightCycle.dayCount + " - ";
        _timeText.text = _dayNightCycle.currentHour + "h";
    }
}
