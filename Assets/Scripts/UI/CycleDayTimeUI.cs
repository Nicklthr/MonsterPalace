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
        _dayNightCycle = FindObjectOfType<DayNightCycle>();
        _dayNightCycle.OnNextDayAction += UpdateDayUI;

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

        _dayText.text = _dayNightCycle.currentDayOfTheWeek + " -";
    }

    void Update()
    {
        _timeText.text = _dayNightCycle.currentHour + "h";
    }

    void UpdateDayUI()
    {
        
        TextMeshFader.Instance.FadeTextWithUpdate( _dayText, LanguageHandler.Instance.GetTranslation(_dayNightCycle.currentDayOfTheWeek), 1f );
    }
}
