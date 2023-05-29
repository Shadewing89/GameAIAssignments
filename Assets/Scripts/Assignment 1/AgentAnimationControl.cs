using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Script tries to link navmesh movement and situations to correct animation

public class AgentAnimationControl : MonoBehaviour
{
    public Animator anim;
    private Vector3 previousPos;
    private bool moving;
    private bool isMoving;
    private bool isCollecting;
    void Start()
    {
        isMoving = false;
        isCollecting = false;
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
        if(move != isMoving && !isCollecting)
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
            isCollecting = true; //stop walking
            ResetAnimatorBool();
            anim.SetBool("Crouch", true); //gather carrot -animation
            StartCoroutine("CrouchTimer", other);
        }
    }
    IEnumerator CrouchTimer(Collider other)
    {
        yield return new WaitForSeconds(1f);
        other.gameObject.SetActive(false); //hide carrot
        isCollecting = false;
        ResetAnimatorBool();
    }
}