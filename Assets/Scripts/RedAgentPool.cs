using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AgentPool will objectpool agents in order to spawn a new agent if a previous one is killed
public class RedAgentPool : MonoBehaviour
{
    public static RedAgentPool SharedInstance;
    public List<GameObject> pooledObjects; //list that gathers the pool
    public List<GameObject> objectsToPool; //What objects we will pool
    private int amountOfSquads; //how many different types of groups we need to create to the list, automatically sized at Start
    public int sizeOfSquad; //size of the pooled units of single group/squad of gameobjects

    void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        amountOfSquads = objectsToPool.Count; //We set the amount of gameObject groups that are pooled from the list objectsToPool set in inspector
        //we create a pool list and then create object into it and deactivate them
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int n = 0; n < amountOfSquads; n++) //We create an amount of individual gameObject and then the same amount of the next gameObject in the list
        {
            for (int i = 0; i < sizeOfSquad; i++)
            {
                tmp = Instantiate(objectsToPool[n]);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
        }
    }
    public GameObject GetPooledObject() //call to use pooled object
    {
        for (int i = 0; i < sizeOfSquad; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
