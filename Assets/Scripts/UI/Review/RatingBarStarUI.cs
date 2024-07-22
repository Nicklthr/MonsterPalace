using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingBarStarUI : MonoBehaviour
{
    public List<Slider> _sliders = new List<Slider>();

    public void UpdateBar(float note)
    {
        int noteInt = (int)note;
        float noteFloat = note - noteInt;

        for (int i = 0; i < _sliders.Count; i++)
        {
            if (i < noteInt)
            {
                _sliders[i].value = 1;
            }
            else if (i == noteInt)
            {
                _sliders[i].value = noteFloat;
            }
            else
            {
                _sliders[i].value = 0;
            }
        }

    }
}
