using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//This script will set the destination for the agent's navmesh navigation
public class AgentNavToDestination : MonoBehaviour
{
    public Transform navigationTarget;
    private NavMeshAgent agent;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        agent.SetDestination(navigationTarget.position + navigationTarget.GetComponent<Collider>().bounds.size); //collider size stops movement to the edge of object
    }
}
