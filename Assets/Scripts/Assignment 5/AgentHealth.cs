using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script handles the agent's health, and kills it to pool at death
public class AgentHealth : MonoBehaviour, IHealth
{
    public float healthMax;
    private float currentHealth;
    private A5_NavScript agentNavScript;

    void Start()
    {
        agentNavScript = GetComponent<A5_NavScript>();
    }
    void Awake()
    {
        if (agentNavScript == null)
        {
            Debug.Log("was null");
            agentNavScript = GetComponent<A5_NavScript>();
        }
        currentHealth = healthMax;
        //agentNavScript.DestinationCheck();
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
        if (this.gameObject == isActiveAndEnabled)
        {
            this.gameObject.SetActive(false);
        }
    }
}
