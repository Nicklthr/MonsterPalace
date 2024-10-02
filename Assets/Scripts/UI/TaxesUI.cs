using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaxesUI : MonoBehaviour
{

    private TMP_Text text;
    public DailyTaxes dailyTaxes;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = dailyTaxes.taxesPrice.ToString()+"$";
    }
}
