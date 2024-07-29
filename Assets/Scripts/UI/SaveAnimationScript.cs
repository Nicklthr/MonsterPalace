using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveAnimationScript : MonoBehaviour
{
    public Image saveIcon; 
    public float fadeDuration = 0.5f;
    public float rotationDuration = 2f;
    public float rotationSpeed = 360f;

    private void Start()
    {
        if (saveIcon != null)
        {
            Color startColor = saveIcon.color;
            startColor.a = 0f;
            saveIcon.color = startColor;
        }
    }

    [ContextMenu("Play Save Animation")]
    public void PlaySaveAnimation()
    {
        StartCoroutine(AnimateSaveIcon());
    }

    private IEnumerator AnimateSaveIcon()
    {
        // Fade In
        yield return StartCoroutine(FadeImage(true));

        // Rotation
        float elapsedTime = 0f;
        while (elapsedTime < rotationDuration)
        {
            saveIcon.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Fade Out
        yield return StartCoroutine(FadeImage(false));
    }

    private IEnumerator FadeImage(bool fadeIn)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = fadeIn ? Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration)
                                 : Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            saveIcon.color = new Color(saveIcon.color.r, saveIcon.color.g, saveIcon.color.b, alpha);
            yield return null;
        }
    }
}