using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 1f;
    public float volume = 0.5f;

    private Coroutine fadeCoroutine;
    private Coroutine fadeVolumeCoroutine;

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
        audioSource.PlayDelayed(0.2f);

        if (fadeIn)
        {
            fadeCoroutine = StartCoroutine(FadeMusic(0f, volume, fadeDuration));
        }
        else
        {
            audioSource.volume = volume;
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
            fadeCoroutine = StartCoroutine(FadeMusic(audioSource.volume, 0f, fadeDuration));
        }
        else
        {
            audioSource.Stop();
        }
    }

    private IEnumerator FadeMusic(float startVolume, float endVolume, float fadeDurationTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDurationTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, elapsedTime / fadeDurationTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        audioSource.volume = endVolume;

        if (endVolume == 0f)
        {
            audioSource.Stop();
        }
    }

    public void ChangeVolume(float newVolume)
    {
        if (fadeVolumeCoroutine != null)
        {
            StopCoroutine(fadeVolumeCoroutine);
        }

        fadeVolumeCoroutine = StartCoroutine(ChangeVolumeCoroutine(newVolume, 0.5f));
    }

    private IEnumerator ChangeVolumeCoroutine(float newVolume, float duration)
    {
        float elapsedTime = 0f;
        float startVolume = audioSource.volume;

        if (duration <= 0f)
        {
            audioSource.volume = newVolume;
            yield break;
        }

        while (elapsedTime < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, newVolume, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        audioSource.volume = newVolume;
    }
}