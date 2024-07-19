using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairCaseController : MonoBehaviour
{
    [SerializeField] private GameObject _stair;
    [SerializeField] private GameObject _stairWall;
    [SerializeField] private GameObject _ground;

    public void CreateStairUp()
    {
        _stairWall.SetActive(false);
        _stair.SetActive( true );
    }

    public void CreateStairDown()
    {
        _stairWall.SetActive( false );
        _ground.SetActive( true );
        _stair.SetActive( false );
    }

    public void DesactivateGround()
    {
       _ground.SetActive( false );
    }

    public void ActivateStair()
    {
       _stair.SetActive( true );
    }
}
