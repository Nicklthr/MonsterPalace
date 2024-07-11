using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPool", menuName = "MonsterPalace/ObjectPool")]
public class SO_ObjectPool : ScriptableObject
{
    public string poolName;
    public GameObject prefab;
    [Tooltip("Taille initiale du pool")]
    [Range(0, 50)]
    public int initialSize;

    [Space(10)]
    [Header("Affichage stat du pool")]
    public int objectsActive = 0;
    public int objectsInactive = 0;

    private void OnEnable()
    {
        ResetPool();
    }

    private void ResetPool()
    {
        objectsActive = 0;
        objectsInactive = 0;
    }
}
