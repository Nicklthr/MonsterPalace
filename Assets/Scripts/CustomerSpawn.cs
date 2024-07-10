using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawn : MonoBehaviour
{
    public MonsterType monsterType;

    void Start()
    {
        RandomSpece();
    }

    void Update()
    {
        
    }

    public void RandomSpece()
    {
        monsterType = (MonsterType)Random.Range(0, System.Enum.GetValues(typeof(MonsterType)).Length);
        Debug.Log(monsterType);
    }
}
