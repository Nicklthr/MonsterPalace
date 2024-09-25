using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private float openAngle = -120f;
    [SerializeField] private float openDuration = 1f;

    [SerializeField] private AudioSource audioSource;

    private bool isOpening = false;

    void Start()
    {
        if (!audioSource)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
    }

    public void OpenTheGate()
    {
        StartCoroutine(OpenGate());
    }

    // Function to rotate doors on Y axis with a lerp over a given time
    public IEnumerator OpenGate()
    {
        if ( isOpening ) yield break;

        isOpening = true;
        float elapsedTime = 0f;
        Quaternion leftStartRotation = leftDoor.transform.rotation;
        Quaternion rightStartRotation = rightDoor.transform.rotation;
        Quaternion leftTargetRotation = Quaternion.Euler(0f, openAngle, 0f);
        Quaternion rightTargetRotation = Quaternion.Euler(0f, -openAngle, 0f);

        audioSource.Play();

        while (elapsedTime < openDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / openDuration;

            leftDoor.transform.rotation = Quaternion.Slerp(leftStartRotation, leftTargetRotation, t);
            rightDoor.transform.rotation = Quaternion.Slerp(rightStartRotation, rightTargetRotation, t);

            yield return null;
        }

        // Ensure the doors reach their final positions
        leftDoor.transform.rotation = leftTargetRotation;
        rightDoor.transform.rotation = rightTargetRotation;

        isOpening = false;
    }
}