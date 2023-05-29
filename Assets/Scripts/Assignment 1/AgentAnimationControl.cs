using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Script tries to link navmesh movement and situations to correct animation

public class AgentAnimationControl : MonoBehaviour
{
    public Animator anim;
    Vector3 previousPos;
    private bool moving;
    void Start()
    {

    }

    void Update()
    {
        moving = previousPos != transform.position;
        anim.SetBool("Walk", moving);
        previousPos = transform.position;
    }


}