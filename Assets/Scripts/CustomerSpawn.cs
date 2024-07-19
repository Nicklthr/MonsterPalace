using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawn : MonoBehaviour
{
    [SerializeField]
    private List<SO_ObjectPool> _monsterPrefabs;

    public DayNightCycle cycle;

    public PoolManager poolManager;
    private GameObject _gameObject;

    public SO_HotelRating hotelRating;

    private List<float> hours = new List<float>();
    public List<Vector2> spawnMinMax;

    private void Update()
    {
        if (hours.Count > 0)
        {
            foreach (float hour in hours)
            {
                if (cycle.currentHour >= hour)
                {
                    SpawnMonster(transform.position);
                    hours.Remove(hour);
                    return;
                }
            }
        }
    }

    public void SpawnMonster(Vector3 position)
    {
        int index = Random.Range(0, _monsterPrefabs.Count);
        var selectedMonster = _monsterPrefabs[index];

        _gameObject = poolManager.GetObjectFromPool(selectedMonster.poolName);
        _gameObject.transform.position = position;

        //Debug.Log("Spawning " + selectedMonster);
    }

    public void Generate()
    {
        int clientNumberToSpawn = (int)Random.Range(spawnMinMax[hotelRating.currentStartRating].x, spawnMinMax[hotelRating.currentStartRating].y);

        hours = new List<float>();

        for (int i = 0; i < clientNumberToSpawn; i++)
        {
            float hourToSpawn = Random.Range(0f, 23f);
            hours.Add(hourToSpawn);
        }
    }
}
