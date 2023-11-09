using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolEntry
    {
        public GameObject prefab;
        public int poolSize;
    }

    [SerializeField] private List<PoolEntry> poolEntries;
    private Dictionary<GameObject, List<GameObject>> pooledObjects;

    private void Start()
    {
        pooledObjects = new Dictionary<GameObject, List<GameObject>>();

        // Create pools for each prefab
        foreach (var entry in poolEntries)
        {
            GameObject prefab = entry.prefab;
            int poolSize = entry.poolSize;

            List<GameObject> objectList = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectList.Add(obj);
            }
            pooledObjects.Add(prefab, objectList);
        }
    }

    public GameObject GetObjectFromPool(GameObject prefab)
    {
        // Find and return an inactive object from the pool
        if (pooledObjects.ContainsKey(prefab))
        {
            foreach (GameObject obj in pooledObjects[prefab])
            {
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }

            // If all objects in the pool are active, create a new one
            GameObject newObj = Instantiate(prefab);
            newObj.SetActive(true);
            pooledObjects[prefab].Add(newObj);
            return newObj;
        }

        return null;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        // Deactivate the object and return it to the pool
        obj.SetActive(false);
    }
}
