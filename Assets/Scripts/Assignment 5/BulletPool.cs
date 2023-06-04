using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BulletPool will objectpool bullets that spawn when a turret shoots
public class BulletPool : MonoBehaviour
{
    public static BulletPool SharedInstance;
    public List<GameObject> pooledObjects; //list that gathers the pool
    public GameObject objectToPool; //What object we will pool
    public int amountToPool; //size of the pool

    void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        //we create a pool list and then create object into it and deactivate them
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    public GameObject GetPooledObject() //call to use pooled object
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}