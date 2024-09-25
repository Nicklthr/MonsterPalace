using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour
{
    [SerializeField] private Vector3 targetLocalPosition;
    [SerializeField] private float moveDuration = 3f;
    [SerializeField] private float xMoveDuration = 1.5f; // Durée fixe pour le mouvement en x
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private bool isMoving = false;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }

    public void StartIntro()
    {
        StartCoroutine(MoveCameraWithRotation());
    }

    public IEnumerator MoveCameraWithRotation()
    {
        if (isMoving) yield break;
        isMoving = true;

        float elapsedTime = 0f;
        float xElapsedTime = 0f;
        Quaternion targetLocalRotation = Quaternion.Euler(0f, 180f, 0f);
        float initialX = initialLocalPosition.x;
        float targetX = targetLocalPosition.x;

        while (elapsedTime < moveDuration || xElapsedTime < xMoveDuration)
        {
            elapsedTime += Time.deltaTime;
            xElapsedTime += Time.deltaTime;

            float t = elapsedTime / moveDuration;
            float xt = Mathf.Clamp01(xElapsedTime / xMoveDuration);

            Vector3 newPosition = Vector3.Lerp(initialLocalPosition, targetLocalPosition, t);
            newPosition.x = Mathf.Lerp(initialX, targetX, xt);

            transform.localPosition = newPosition;
            transform.localRotation = Quaternion.Slerp(initialLocalRotation, targetLocalRotation, t);

            yield return null;
        }

        // S'assurer que la caméra atteint sa position et rotation finales
        transform.localPosition = targetLocalPosition;
        transform.localRotation = targetLocalRotation;
        isMoving = false;
    }

    public void ResetCamera()
    {
        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;
    }
}