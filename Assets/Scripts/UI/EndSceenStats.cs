using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSceenStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playedTime;
    [SerializeField] private TextMeshProUGUI _coinsGranted;
    [SerializeField] private TextMeshProUGUI _customersWelcomed;

    [SerializeField] private float _customersWelcomedCount;

    [SerializeField] private RunManager _runManager;

    public static EndSceenStats Instance {  get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        _playedTime.text = _runManager.runChrono.ToString();
        _coinsGranted.text = _runManager.coinsGranted.ToString();
        _customersWelcomed.text = _customersWelcomedCount.ToString();
    }

    public void CustomersCountAdd(float value)
    {
        _customersWelcomedCount += value;
    }
}
