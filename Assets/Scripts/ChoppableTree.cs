using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles choppable trees' health, death, and notifying navigation that they have died with event call
public class ChoppableTree : MonoBehaviour, IHealth
{
    public float healthMax;
    private float currentHealth;
    public GameObject treeTop;
    public Transform treeLogSpawnPoint;
    private bool shouldLogSpawn;
    private bool deathEventCalled;
    public delegate void TreeDeath();
    public static event TreeDeath OnTreeDeath;
    void Start()
    {
        currentHealth = healthMax;
        treeTop.SetActive(true);
        if (LogPool.SharedInstance == null)
        {
            shouldLogSpawn = false;
        }
        else if (LogPool.SharedInstance != null)
        {
            shouldLogSpawn = true;
        }
        deathEventCalled = false;
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
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
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
        if (OnTreeDeath != null && !deathEventCalled) //if event is not null, call event to inform agents of disappearance of tree and spawn of collectable
        {
            OnTreeDeath();
            Debug.Log("Tree called ontreedeath");
            deathEventCalled = true;
        }
    }
}
