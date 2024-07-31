using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string tooltipContent;
    [SerializeField] private bool useTag = false;
    [SerializeField] private string tooltipTag = "Tooltip";

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!useTag || gameObject.CompareTag(tooltipTag))
        {
            Vector3 screenPosition = Input.mousePosition;
            TooltipSystem.Instance.ShowTooltip(tooltipContent, screenPosition);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Instance.HideTooltip();
    }
}

public static class TooltipEvents
{
    public static event Action<string, Vector3> OnShowTooltip;
    public static event Action OnHideTooltip;

    public static void TriggerShowTooltip(string content, Vector3 position)
    {
        OnShowTooltip?.Invoke(content, position);
    }

    public static void TriggerHideTooltip()
    {
        OnHideTooltip?.Invoke();
    }
}