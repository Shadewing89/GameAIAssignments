using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;

//This script will set the destination for the agent's navmesh navigation
//in a5 we need to add ability to find choppable trees
public class A5_NavScript : MonoBehaviour
{
    public List<Transform> collectableTargets;
    public List<Transform> pushTargets;
    public List<Transform> treeTargets;
    public List<Transform> harvestTargets;
    private NavMeshAgent agent;
    private int nextDestinationIndex;
    private int nextPushIndex;
    private bool availableCollectables;
    private bool availablePushables;
    private bool availableTrees;
    private bool availableHarvestables; //for later use with carrots in assignment 8

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        availableCollectables = false;
        availablePushables = false;
        availableTrees = false;
        availableHarvestables = false;
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
                collectableTargets.Add(go.transform);
                availableCollectables = true;
            }
            if (go.layer == 8 && go.transform.parent == null)
            {
                pushTargets.Add(go.transform);
                availablePushables = true;
            } 
            if (go.layer == 11 && go.transform.parent == null)
            {
                treeTargets.Add(go.transform);
                availableTrees = true;
            }
        }
        if (collectableTargets.Count == 0)
        {
            availableCollectables = false;
        }
        if (pushTargets.Count == 0)
        {
            availablePushables = false;
        }
        if (treeTargets.Count == 0)
        {
            availableTrees = false;
        }
        if (harvestTargets.Count == 0)
        {
            availableHarvestables = false;
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
        if (availableCollectables)
        {
            CollectableObjectCheck();
        }
        else if (availableTrees)
        {
            TreeCheck();
        }
        else if (availablePushables)
        {
            PushableObjectCheck();
        }
        else
        {
            StopMoving();
        }
    }
    public void CollectableObjectCheck()
    {
        for (int i = 0; i < collectableTargets.Count; ++i)
        {
            if (collectableTargets[i].gameObject.activeInHierarchy == true)
            {
                nextDestinationIndex = i;
                break;
            }
        }
        if (collectableTargets[nextDestinationIndex].gameObject.activeInHierarchy == false) //if no more collectables
        {
            availableCollectables = false;
        }
        else //find next destination to set
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(collectableTargets[nextDestinationIndex].position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(collectableTargets[nextDestinationIndex].position);
            }
            else
            {
                availableCollectables = false;
                DestinationCheck();
            }
        }
    }
    public void TreeCheck()
    {
        for (int i = 0; i < treeTargets.Count; ++i)
        {
            if (treeTargets[i].gameObject.activeInHierarchy == true)
            {
                nextDestinationIndex = i;
                break;
            }
        }

        if (treeTargets[nextDestinationIndex].gameObject.activeInHierarchy == false) //if no more collectables
        {
            availableTrees = false;
        }
        else //find next destination to set
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(treeTargets[nextDestinationIndex].position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(treeTargets[nextDestinationIndex].position);
            }
            else
            {
                availableTrees = false;
                DestinationCheck();
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
        else //later replace with other backup
        {
            nextPushIndex++;
            if (nextPushIndex > pushTargets.Count)
            {
                availablePushables = false;
                DestinationCheck();
            }
            //else
            //{
            //    PushableObjectCheck(); //second look into the code shows that this achieves nothing in current context
            //}
        }
    }
    public void TempDestination(GameObject tempWaypoint) //for stuff like reorienting the direction of box pushing by walking to another side before continuing the previous path
    {
        agent.SetDestination(tempWaypoint.transform.position);
    }
    void OnEnable() //register to events
    {
        Debug.Log("OnEnable");
        ChoppableTree.OnTreeDeath += TreeHasDied;
        Debug.Log("OnEnable called descheck");
    }
    void OnDisable() //unsubscribe from events
    {
        ChoppableTree.OnTreeDeath -= TreeHasDied;
    }
    private void TreeHasDied()
    {
        Debug.Log("event activated");
        treeTargets.Clear();
        ListNavTargets();
        DestinationCheck();
    } 
}
