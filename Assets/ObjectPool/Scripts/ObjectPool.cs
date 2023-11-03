using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;

    public List<GameObject> objectsToPool;
    public Dictionary<string, List<GameObject>> listsToPool;
    private int amountToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        listsToPool = new Dictionary<string, List<GameObject>>();
        GameObject poolObject;

        foreach (GameObject gameObject in objectsToPool)
        {
            string name = gameObject.GetComponent<Projectile>().type.ToString();
            amountToPool = gameObject.GetComponent<Projectile>().poolAmmount;

            List<GameObject> pooledObjects = new List<GameObject>();
            for (int i = 0; i < amountToPool; i++)
            {
                poolObject = Instantiate(gameObject);
                poolObject.SetActive(false);
                pooledObjects.Add(poolObject);
            }

            listsToPool.Add(name, pooledObjects);
        }
    }

    public GameObject GetPooledObject(string bulletType)
    {
        if (bulletType == null)
        {
            return null;
        }

        for (int i = 0; i < amountToPool; i++)
        {
            if (!listsToPool[bulletType][i].activeInHierarchy)
            {
                return listsToPool[bulletType][i];
            }
        }
        return null;
    }
}
