using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Gère le fade in/out d'un TextMeshPro avec mise à jour du contenu.
/// Cette classe utilise un CanvasGroup pour contrôler l'alpha du texte et l'ajoute automatiquement si nécessaire.
/// </summary>
public class TextMeshFader : MonoBehaviour
{
    private static TextMeshFader _instance;

    /// <summary>
    /// Instance singleton de TextMeshFader.
    /// </summary>
    public static TextMeshFader Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("TextMeshFader");
                _instance = go.AddComponent<TextMeshFader>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    /// <summary>
    /// Effectue un fade out, met à jour le texte, puis fait un fade in.
    /// Ajoute automatiquement un CanvasGroup si nécessaire.
    /// </summary>
    /// <param name="textMesh">Le TextMeshProUGUI à animer.</param>
    /// <param name="newText">Le nouveau texte à afficher.</param>
    /// <param name="fadeDuration">La durée de chaque fade (in et out).</param>
    /// <param name="delayBetweenFades">Le délai entre le fade out et le fade in.</param>
    public void FadeTextWithUpdate(TextMeshProUGUI textMesh, string newText, float fadeDuration = 0.5f, float delayBetweenFades = 0.1f)
    {
        CanvasGroup canvasGroup = EnsureCanvasGroup(textMesh);
        StartCoroutine(FadeTextCoroutine(textMesh, canvasGroup, newText, fadeDuration, delayBetweenFades));
    }

    /// <summary>
    /// Vérifie la présence d'un CanvasGroup et en ajoute un si nécessaire.
    /// </summary>
    /// <param name="textMesh">Le TextMeshProUGUI pour lequel vérifier/ajouter un CanvasGroup.</param>
    /// <returns>Le CanvasGroup existant ou nouvellement ajouté.</returns>
    private CanvasGroup EnsureCanvasGroup(TextMeshProUGUI textMesh)
    {
        CanvasGroup canvasGroup = textMesh.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = textMesh.gameObject.AddComponent<CanvasGroup>();
        }
        return canvasGroup;
    }

    /// <summary>
    /// Coroutine qui gère l'animation de fade out, la mise à jour du texte, et le fade in.
    /// </summary>
    private IEnumerator FadeTextCoroutine(TextMeshProUGUI textMesh, CanvasGroup canvasGroup, string newText, float fadeDuration, float delayBetweenFades)
    {
        // Fade Out
        yield return StartCoroutine(FadeCoroutine(canvasGroup, canvasGroup.alpha, 0f, fadeDuration));

        // Mise à jour du texte
        textMesh.text = newText;

        // Délai entre les fades
        yield return new WaitForSeconds(delayBetweenFades);

        // Fade In
        yield return StartCoroutine(FadeCoroutine(canvasGroup, canvasGroup.alpha, 1f, fadeDuration));
    }

    /// <summary>
    /// Coroutine qui gère l'animation de fade (in ou out).
    /// </summary>
    private IEnumerator FadeCoroutine(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
}