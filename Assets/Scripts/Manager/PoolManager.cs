using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Composant PoolsManager pour gérer les pools d'objets dans le jeu
/// </summary>
[AddComponentMenu("MonsterPalace/PoolsManager")]
[DisallowMultipleComponent]
public class PoolManager : MonoBehaviour
{
    #region Public Variables
    [System.Serializable]
    public class Pool
    {
        public SO_ObjectPool SO_pool;
        public List<GameObject> poolObjects;
    }

    public Pool[] pools;
    public bool debugMode = false;
    #endregion

    #region Private Variables
    private Dictionary<string, Pool> poolDictionary;
    #endregion

    #region Unity Lifecycle
    void Start()
    {
        poolDictionary = new Dictionary<string, Pool>();
        InitializePools();
    }
    #endregion

    #region Pool Methods
    /// <summary>
    /// Initialisation des pools de la liste des pools
    /// </summary>
    void InitializePools()
    {
        foreach (var pool in pools)
        {
            GameObject poolParent = new GameObject("Pool_" + pool.SO_pool.poolName);
            poolParent.transform.parent = transform;

            pool.poolObjects = new List<GameObject>();

            for (int i = 0; i < pool.SO_pool.initialSize; i++)
            {
                GameObject obj = Instantiate(pool.SO_pool.prefab);
                obj.SetActive(false);
                
                MonsterController monstreController = obj.GetComponent<MonsterController>();

                if (monstreController != null)
                {
                    int idObj = obj.GetInstanceID();
                    monstreController.monsterID = idObj.ToString() + "_" + i.ToString();
                }

                obj.transform.parent = poolParent.transform;
                pool.poolObjects.Add(obj);
                pool.SO_pool.objectsInactive++;
            }

            poolDictionary.Add(pool.SO_pool.poolName, pool);
        }

        LogDebug("Les pools ont été initialisées");
    }

    /// <summary>
    /// Fonction de récupération d'un objet depuis la pool pour l'utiliser
    /// </summary>
    /// <param name="poolName"> Donner le nom de la pool à utiliser </param>
    /// <returns> Retourne un objet de la pool </returns>
    public GameObject GetObjectFromPool(string poolName)
    {
        if (poolDictionary.ContainsKey(poolName))
        {
            foreach (var obj in poolDictionary[poolName].poolObjects)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    poolDictionary[poolName].SO_pool.objectsActive++;
                    poolDictionary[poolName].SO_pool.objectsInactive--;
                    LogDebug("Objet obtenu depuis la pool " + poolName);
                    return obj;
                }
            }

            LogDebug("Plus d'objet disponible dans la pool " + poolName);
            return null;
        }

        LogDebug("La Pool : " + poolName + " n'existe pas.");
        return null;
    }

    /// <summary>
    /// Fonction de retour d'un objet dans le pool pour le réutiliser
    /// </summary>
    /// <param name="obj">Objet a retourner dans le pool</param>
    public void ReturnObjectToPool(GameObject obj, string poolName)
    {
        // On vérifie si l'objet appartient bien à la pool
        if (poolDictionary.ContainsKey(poolName))
        {

            obj.SetActive(false);

            poolDictionary[poolName].SO_pool.objectsActive--;
            poolDictionary[poolName].SO_pool.objectsInactive++;
            LogDebug("Objet retourné dans la pool " + poolName);
        }
        else
        {
            LogDebug("L'objet ne peut pas être retourné dans la pool " + poolName + " car il n'appartient pas à cette pool");
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Fonction de log en mode debug pour afficher des messages dans la console Unity
    /// </summary>
    /// <param name="message"> Réceptionne le message à afficher </param>
    /// <remarks> Cette fonction n'est appelée que si le mode debug est activé </remarks>
    private void LogDebug(string message)
    {
        if (debugMode)
        {
            Debug.Log(message);
        }
    }
    #endregion
}
