using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//This script will set the destination for the agent's navmesh navigation
//in a2 we need to gather multiple collectables into a list and collect them all
public class A2_AgentNavToDestination : MonoBehaviour
{
    public List<Transform> navigationTargets;
    private NavMeshAgent agent;
    private int nextDestinationIndex;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        ListNavTargets();
        DestinationCheck();
    }
    void ListNavTargets()
    {
        //https://answers.unity.com/questions/968709/find-object-by-layer-unity-5-split-screen-camera-f.html
        GameObject[] gos = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in gos)
        {
            if (go.layer == 7 && go.transform.parent == null)//&& go.transform.parent == null makes sure we only add the parent objects in hierarchy, not the children
            {
                navigationTargets.Add(go.transform);
            }
        }
    }
    void Update()
    {
        
    }
    public void StopMoving()//set the destination to current position so the agent stops
    {
        agent.SetDestination(transform.position);
    }
    public void DestinationCheck()
    {
        for (int i = 0; i < navigationTargets.Count; ++i)
        {
            if(navigationTargets[i].gameObject.activeInHierarchy == true)
            {
                nextDestinationIndex = i;
                break;
            }
        }
        agent.SetDestination(navigationTargets[nextDestinationIndex].position);
    }
}
