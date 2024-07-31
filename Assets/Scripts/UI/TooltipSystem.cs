using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;

    [SerializeField] private GameObject tooltipPrefab;
    private GameObject tooltipInstance;
    private Text tooltipText;

    public static TooltipSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TooltipSystem>();
                if (instance == null)
                {
                    GameObject go = new GameObject("TooltipSystem");
                    instance = go.AddComponent<TooltipSystem>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        tooltipInstance = Instantiate(tooltipPrefab, transform);
        //tooltipText = tooltipInstance.GetComponentInChildren<Text>();
        tooltipInstance.SetActive(false);
    }

    public void ShowTooltip(string content, Vector3 position)
    {
        tooltipInstance.SetActive(true);
        //tooltipText.text = content;
        tooltipInstance.transform.position = position;
    }

    public void HideTooltip()
    {
        tooltipInstance.SetActive(false);
    }
}