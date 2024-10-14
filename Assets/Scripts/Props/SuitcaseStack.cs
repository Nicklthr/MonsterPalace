using UnityEngine;

public class SuitcaseStack : MonoBehaviour
{
    [SerializeField] private float levitationHeight = 1.0f;
    [SerializeField] private float oscillationAmplitude = 0.1f;
    [SerializeField] private float oscillationSpeed = 1.0f;
    [SerializeField] private float rotationSpeed = 5.0f;

    private float initialY;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // Initialiser la position
        initialPosition = transform.position;
        initialY = initialPosition.y;

        // Définir la hauteur de lévitation
        transform.position = new Vector3(initialPosition.x, levitationHeight, initialPosition.z);

        // Rotation aléatoire sur l'axe Y pour chaque enfant
        foreach (Transform child in transform)
        {
            float randomYRotation = Random.Range(0f, 360f);
            child.rotation = Quaternion.Euler(0f, randomYRotation, 0f);
        }

        // Enregistrer la rotation initiale
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Oscillation aléatoire et légère sur Y
        float newY = levitationHeight + Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

        // Légère rotation lente sur Y
        transform.rotation = initialRotation * Quaternion.Euler(0f, Time.time * rotationSpeed, 0f);
    }
}