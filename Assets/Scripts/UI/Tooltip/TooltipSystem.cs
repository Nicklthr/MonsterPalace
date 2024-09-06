using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public Tooltip tooltip;
    private CanvasGroup canvasGroup;

    public float fadeDuration = 0.3f;

    public void Awake()
    {
        current = this;
        canvasGroup = tooltip.GetComponent<CanvasGroup>();
    }

    public static void Show(string content, string header= "")
    {
        current.StopAllCoroutines();
        current.tooltip.SetText(content, header);
        current.tooltip.gameObject.SetActive(true);
        current.StartCoroutine(current.FadeIn());
    }

    public static void Hide()
    {
        current.StopAllCoroutines();
        current.StartCoroutine(current.FadeOut());
        current.tooltip.gameObject.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForEndOfFrame();

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime / fadeDuration;
            canvasGroup.alpha = progress;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float progress = 1;
        while (progress > 0)
        {
            progress -= Time.deltaTime / fadeDuration;
            canvasGroup.alpha = progress;
            yield return null;
        }
    }
}
