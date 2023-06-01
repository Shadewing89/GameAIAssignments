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
    [SerializeField] private GameObject goalPlusX;
    [SerializeField] private GameObject goalMinusX;
    [SerializeField] private GameObject goalPlusZ;
    [SerializeField] private GameObject goalMinusZ;
    private A2_AgentNavToDestination agentNavDes;
    private float distanceToGoal;

    void Start()
    {
        navMeshLink.SetActive(false);
        agentNavDes = GameObject.FindWithTag("Player").GetComponent<A2_AgentNavToDestination>();
        //distanceToGoal = 
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) //check if box is pushed into pit
    {
        if (other.gameObject.layer == 9)
        {
            navMeshLink.SetActive(true);
            agentNavDes.DestinationCheck();
        }
    }
    private void OnCollisionEnter(Collision collision) //check if box is pushed into wall
    {
        if(collision.gameObject.layer == 10)
        {

        }
    }
}
