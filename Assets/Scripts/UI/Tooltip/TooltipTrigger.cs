using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string content;
    public string header;
    public float delay = 0.9f;

    private Coroutine showTooltipCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        showTooltipCoroutine = StartCoroutine(ShowTooltipAfterDelay());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showTooltipCoroutine != null)
        {
            StopCoroutine(showTooltipCoroutine);
            showTooltipCoroutine = null;
        }
        TooltipSystem.Hide();
    }

    private IEnumerator ShowTooltipAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        TooltipSystem.Show(content, header);

        showTooltipCoroutine = null;
    }
}