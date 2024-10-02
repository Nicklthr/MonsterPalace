using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndStatsUI : MonoBehaviour
{
    
    public RunManager manager;
    private TMP_Text text;
    public bool gametime = true;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gametime)
        {
            text.text = TimeSpan.FromSeconds(manager.runChrono).Hours+":"+TimeSpan.FromSeconds(manager.runChrono).Minutes+":"+ TimeSpan.FromSeconds(manager.runChrono).Seconds;
        }
        else
        {
            text.text = manager.coinsGranted.ToString();
        }
    }
}
