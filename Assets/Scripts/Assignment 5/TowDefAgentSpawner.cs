using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will spawn a new agent at regular intervals for the tower defence turrets to shoot
public class TowDefAgentSpawner : MonoBehaviour
{
    public float spawnIntervalSeconds;
    private bool canSpawn;
    
    void Start()
    {
        
        StartCoroutine("SpawnIntervalTimer");
    }

    
    void Update()
    {
        if (canSpawn)
        {
            StartCoroutine("SpawnIntervalTimer");
        }
    }
    IEnumerator SpawnIntervalTimer()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnIntervalSeconds);
        SpawnNewAgentFromPool();
    }
    public void SpawnNewAgentFromPool()
    {
        GameObject agent = RedAgentPool.SharedInstance.GetPooledObject();
        if (agent != null)
        {
            //agent.GetComponent<Rigidbody>().velocity = Vector3.zero; //This resets the movement of the gameobject
            //agent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            agent.transform.position = gameObject.transform.position;
            agent.transform.rotation = gameObject.transform.rotation;
            agent.SetActive(true);
            canSpawn = true;
        }
    }
}