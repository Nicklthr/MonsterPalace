using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayNightCycle : MonoBehaviour
{
    [Tooltip("Temps pour un jour complet en secondes.")]
    public float dayLength = 120f; // Durée d'un jour en secondes (120s par défaut)

    [Tooltip("Coefficient multiplicateur de la vitesse du cycle jour/nuit.")]
    public float timeMultiplier = 1f; // Coefficient multiplicateur pour ajuster la vitesse

    private float rotationSpeed;

    [SerializeField]
    private float timer = 0f;

    public int dayCount;
    public int currentHour;
    public int startHour = 0;

    public UnityEvent OnNextDay = new UnityEvent();

    

    void Start()
    {
        // Calcul de la vitesse de rotation en degrés par seconde
        rotationSpeed = 360f / dayLength;
        StartingTime();
    }

    void Update()
    {
        // Calcul de la rotation en fonction du temps écoulé et du coefficient multiplicateur
        float rotationThisFrame = rotationSpeed * Time.deltaTime * timeMultiplier;
        transform.Rotate(Vector3.right, rotationThisFrame);

        Clock();
        CurrentTime();
    }

    public void Clock()
    {
        timer += Time.deltaTime;

        if(timer >= dayLength)
        {
            dayCount++;
            timer = 0;
            OnNextDay.Invoke();
        }
    }

    public void CurrentTime()
    {
        float timeRatio = timer / dayLength;

        currentHour = (int)(timeRatio * 24f);
    }

    public void StartingTime()
    {
        float timeRatio = startHour / 24f;

        timer = timeRatio * dayLength;
    }
}
