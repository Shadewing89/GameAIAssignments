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
    //private AnimatorControllerParameter param;
    //List<AnimatorControllerParameter> parameters;
    void Start()
    {
        agentNavDes = GetComponent<A2_AgentNavToDestination>();
        SetAnimatorBool(default);
        //Listing parameters is useful feature and is saved for future use
        ////we check the animator parameters
        //for (int i = 0; i < anim.parameters.Length; i++) //https://answers.unity.com/questions/494966/get-list-of-parameters-of-an-animatorcontroller.html
        //{
        //    param = anim.parameters[i];
        //    Debug.Log("Parameter Name: " + param.name);

        //    if (param.type == AnimatorControllerParameterType.Bool)
        //    {
        //        //Debug.Log("Default Bool: " + param.defaultBool);
        //    }
        //    else if (param.type == AnimatorControllerParameterType.Float)
        //    {
        //        //Debug.Log("Default Float: " + param.defaultFloat);
        //    }
        //    else if (param.type == AnimatorControllerParameterType.Int)
        //    {
        //        //Debug.Log("Default Int: " + param.defaultInt);
        //    }
        //    parameters.Add(param);
        //}
    }

    void Update()
    {
        moving = previousPos != transform.position;
        if (moving)
        {
            SetAnimatorBool("Walk");
        }
        previousPos = transform.position;

    }
    void SetAnimatorBool(string parameterToBeTrue) //We reset every bool to false in the animator and make one true
    {
        foreach (AnimatorControllerParameter parameter in anim.parameters) 
        {
            anim.SetBool(parameter.name, false);
        }
        switch (parameterToBeTrue)
        {
            case "Walk":
                anim.SetBool("Walk", true);
                break;
            case "Crouch":
                agentNavDes.StopMoving();
                anim.SetBool("Crouch", true);
                break;
            case "Harvest":
                agentNavDes.StopMoving();
                anim.SetBool("Harvest", true);
                break;
            case "WoodChop":
                agentNavDes.StopMoving();
                anim.SetBool("WoodChop", true);
                break;
            default:
                //all stay false and agent idles
                break;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7) //layer 7 in inspector is set to collectable, which we want to check
        {   
            SetAnimatorBool("Crouch");
            StartCoroutine("ActionTimer", other);
        }
    }
    IEnumerator ActionTimer(Collider other)
    {
        yield return new WaitForSeconds(1f);
        other.gameObject.SetActive(false); //hide carrot or kill agent being hit
        SetAnimatorBool(default);
        yield return new WaitForSeconds(1f);
        agentNavDes.DestinationCheck();
    }
}