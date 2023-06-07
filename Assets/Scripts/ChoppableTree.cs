using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableTree : MonoBehaviour, IHealth
{
    public float healthMax;
    private float currentHealth;
    public GameObject treeTop;
    public Transform treeLogSpawnPoint;
    private bool shouldLogSpawn;
    void Start()
    {
        if (LogPool.SharedInstance == null)
        {
            shouldLogSpawn = false;
        }
        else if (LogPool.SharedInstance != null)
        {
            shouldLogSpawn = true;
        }
    }
    void Update()
    {
        if (currentHealth > healthMax)
        {
            currentHealth = healthMax;
        }
        else if (currentHealth <= 0f)
        {
            Death();
        }
    }
    public void TakeDamage()
    {

    }
    public void Death()
    {
        treeTop.SetActive(false);
        gameObject.layer = 0; //we change to default layer to get ignored in navigation targeting
        if (shouldLogSpawn)
        {
            GameObject log = LogPool.SharedInstance.GetPooledObject();
            if (log != null)
            {
                log.GetComponent<Rigidbody>().velocity = Vector3.zero; //This resets the movement of the gameobject
                log.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                log.transform.position = treeLogSpawnPoint.position;
                log.transform.rotation = treeLogSpawnPoint.rotation;
                log.SetActive(true);
                //log.GetComponent<Rigidbody>().AddForce(bulletShootSpawnLocation.up * bulletSpeed, ForceMode.Impulse); //impulse to get all the energy at once
            }
        }
    }
}
