using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

//This script will set the destination for the agent's navmesh navigation
//in a2 we need to gather multiple collectables into a list and collect them all
public class A2_AgentNavToDestination : MonoBehaviour
{
    public List<Transform> navigationTargets;
    public List<Transform> pushTargets;
    private NavMeshAgent agent;
    private int nextDestinationIndex;
    private int nextPushIndex;
    
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
            if (go.layer == 8 && go.transform.parent == null)
            {
                pushTargets.Add(go.transform);
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
        
        if (navigationTargets[nextDestinationIndex].gameObject.activeInHierarchy == false) //if no more collectables
        {
            StopMoving();
        }
        else //find next destination to set
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(navigationTargets[nextDestinationIndex].position, path);
            if(path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(navigationTargets[nextDestinationIndex].position);
            }
            else
            {
                PushableObjectCheck();
            }
        }
    }
    public void PushableObjectCheck() //We check for pushable objects that need to be pushed to pressure plates or pits
    {
        for (int i = 0; i < pushTargets.Count; ++i)
        {
            if (pushTargets[i].gameObject.activeInHierarchy == true)
            {
                nextPushIndex = i;
                break;
            }
        }
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(pushTargets[nextPushIndex].position, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(pushTargets[nextPushIndex].position);
        }
        else //brute force another pushable, later replaceable with other backup
        {
            nextPushIndex++;
            if(nextPushIndex >= pushTargets.Count)
            {
                StopMoving();
            }
            else
            {
                PushableObjectCheck();
            }
        }
    }
    public void TempDestination(GameObject tempWaypoint) //for stuff like reorienting the direction of box pushing by walking to another side before continuing the previous path
    {
        agent.SetDestination(tempWaypoint.transform.position);
    }
}
