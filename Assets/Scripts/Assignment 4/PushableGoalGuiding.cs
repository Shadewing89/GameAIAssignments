using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//This script handles the movable object
//It checks the location of the pit and changes the target on a pushable cube so the agent would pathfind to that point
//and therefore hopefully push the cube towards the pit
public class PushableGoalGuiding : MonoBehaviour
{
    public GameObject goal;
    public GameObject navMeshLink;
    public List<GameObject> movablePushSides;
    private A2_AgentNavToDestination agentNavDes;
    private float biggestDist;
    public GameObject furthestPoint;
    public bool whichSideMethodCalled;
    private bool goalReached;

    void Start()
    {
        navMeshLink.SetActive(false);
        agentNavDes = GameObject.FindWithTag("Player").GetComponent<A2_AgentNavToDestination>(); //Only works with one agent
        whichSideMethodCalled = false; //stops infinite loop of checking
        goalReached = false;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) //check if box is pushed into pit
    {
        if (other.gameObject.layer == 9)
        {
            goalReached = true;
            Physics.IgnoreLayerCollision(6, 8); //this will stop collision between agent and pushable box layers, not good if multiple pushables
            navMeshLink.SetActive(true);
            agentNavDes.DestinationCheck();
        }
    }
    private void OnCollisionEnter(Collision collision) //check if box is pushed into wall or player walks into it
    {
        if(collision.gameObject.layer == 10 && !goalReached)
        {
            WhichSideToPush();
        }
        if (collision.gameObject.layer == 6 && !goalReached)
        {
            if (whichSideMethodCalled == false)
            {
                WhichSideToPush();
            }
        }
    }
    private void WhichSideToPush()
    {
        foreach (GameObject side in movablePushSides)
        {
            float distance = Vector3.Distance(side.transform.position, goal.transform.position);
            if(distance > biggestDist)
            {
                biggestDist = distance;
                furthestPoint = side;
            }
        }
        furthestPoint.SetActive(true);
        agentNavDes.TempDestination(furthestPoint);
        whichSideMethodCalled = false; //sets it false, only set true after agent has reached the reorient navpoint in PushNavTargetReached.cs
    }
}
