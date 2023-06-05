using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableTree : MonoBehaviour, IHealth
{
    public float healthMax;
    private float currentHealth;
    public GameObject treeTop;
    public Transform treeLogSpawnPoint;
    void Start()
    {
        
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
    }
}
