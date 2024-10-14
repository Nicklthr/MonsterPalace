using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIndoorController : MonoBehaviour
{

    [SerializeField] private Transform door;

    private bool isOpen = false;

    [SerializeField] private List<string> tags;

    [SerializeField] private AudioClip doorOpenSound, doorCloseSound;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        if (doorOpenSound == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tags.Contains(other.tag))
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (tags.Contains(other.tag))
        {
            CloseDoor();
        }
    }

    // fonction pour ouvrir la porte ou les portes
    public void OpenDoor()
    {
        if (!isOpen)
        {
            door.Rotate(0, 0, 0);
            isOpen = true;
            audioSource.PlayOneShot(doorOpenSound);
        }
    }

    // fonction pour fermer la porte ou les portes
    public void CloseDoor()
    {
        if (isOpen)
        {
            door.Rotate(0, -90, 0);
            isOpen = false;
            audioSource.PlayOneShot(doorCloseSound);
        }
    }

}
