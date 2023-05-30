using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Script tries to link navmesh movement and situations to correct animation

public class AgentAnimationControl : MonoBehaviour
{
    private A2_AgentNavToDestination agentNavDes;
    public Animator anim;
    private Vector3 previousPos;
    private bool moving;
    private bool isMoving;
    private bool isActing;

    void Start()
    {
        agentNavDes = GetComponent<A2_AgentNavToDestination>();
        isMoving = false;
        isActing = false;
        ResetAnimatorBool();
    }

    void Update()
    {
        moving = previousPos != transform.position;
        AnimateWalk(moving);
        previousPos = transform.position;

    }
    void ResetAnimatorBool() //We reset every bool to false in the animator
    {
        foreach (AnimatorControllerParameter parameter in anim.parameters) 
        {
            anim.SetBool(parameter.name, false);
        }
    }
    void AnimateWalk(bool move)
    {
        if(move != isMoving && !isActing)
        {
            ResetAnimatorBool();
            anim.SetBool("Walk", move);
            isMoving = move;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7) //layer 7 in inspector is set to collectable, which we want to check
        {
            isActing = true; //stop walking -animation
            agentNavDes.StopMoving();
            ResetAnimatorBool();
            anim.SetBool("Crouch", true); //gather carrot -animation
            StartCoroutine("ActionTimer", other);
        }
    }
    IEnumerator ActionTimer(Collider other)
    {
        yield return new WaitForSeconds(1f);
        other.gameObject.SetActive(false); //hide carrot
        isActing = false;
        ResetAnimatorBool();
        yield return new WaitForSeconds(1f);
        agentNavDes.DestinationCheck();
    }
}