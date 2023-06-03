using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will check if the agent has reached its collider trigger and reorients its navigation to the pushable cube parent
public class PushNavTargetReached : MonoBehaviour
{
    private A2_AgentNavToDestination agentNavDes;
    private PushableGoalGuiding parentScript;
    private GameObject parentPushable;
    void Start()
    {
        parentScript = GetComponentInParent<PushableGoalGuiding>();
        parentPushable = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        gameObject.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            agentNavDes = other.GetComponent<A2_AgentNavToDestination>();
            parentScript.whichSideMethodCalled = true;
            agentNavDes.TempDestination(parentPushable);
            gameObject.SetActive(false);
        }
    }

}
