using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 1f;

    private Coroutine fadeCoroutine;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayMusic(AudioClip musicClip, bool fadeIn = true)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        audioSource.clip = musicClip;
        audioSource.Play();

        if (fadeIn)
        {
            fadeCoroutine = StartCoroutine(FadeMusic(0f, 1f));
        }
        else
        {
            audioSource.volume = 1f;
        }
    }

    public void StopMusic(bool fadeOut = true)
    {
        if (fadeOut)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeMusic(audioSource.volume, 0f));
        }
        else
        {
            audioSource.Stop();
        }
    }

    private IEnumerator FadeMusic(float startVolume, float endVolume)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = endVolume;

        if (endVolume == 0f)
        {
            audioSource.Stop();
        }
    }
}