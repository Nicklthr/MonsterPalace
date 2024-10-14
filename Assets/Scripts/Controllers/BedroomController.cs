using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomController : MonoBehaviour
{
    [SerializeField] private GameObject suitcaseStack;

    public void ActivateSuitcase()
    {
        Debug.Log("ActivateSuitcase called");
        if (suitcaseStack != null)
        {
            Debug.Log("SuitcaseStack is assigned, activating it.");
            suitcaseStack.SetActive(true);
        }
        else
        {
            Debug.LogError("BedroomController: SuitcaseStack is not assigned");
        }
    }

    public void DeactivateSuitcase()
    {
        if(suitcaseStack != null)
        {
            suitcaseStack.SetActive(false);
        }
        else
        {
            Debug.LogError("BedroomController: SuitcaseStack is not assigned");
        }
    }
}
