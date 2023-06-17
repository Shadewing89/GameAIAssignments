using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Script tries to link navmesh movement and situations to correct animation

public class AnimationA5 : MonoBehaviour
{
    private A5_NavScript agentNavScript;
    public Animator anim;
    private Vector3 previousPos;
    private bool moving;
    //private AnimatorControllerParameter param;
    //List<AnimatorControllerParameter> parameters;
    void Start()
    {
        agentNavScript = GetComponent<A5_NavScript>();
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
                agentNavScript.StopMoving();
                anim.SetBool("Crouch", true);
                break;
            case "Harvest":
                agentNavScript.StopMoving();
                anim.SetBool("Harvest", true);
                break;
            case "WoodChop":
                agentNavScript.StopMoving();
                anim.SetBool("WoodChop", true);
                break;
            default:
                //all stay false and agent idles
                break;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) //layer 7 in inspector is set to collectable, which we want to check
        {
            SetAnimatorBool("Crouch");
            StartCoroutine("CollectionTimer", other);
        }
        if (other.gameObject.layer == 11 && other.TryGetComponent(out IHealth hit))
        {
            //Debug.Log("Collider to woodchop");
            SetAnimatorBool("WoodChop"); 
            StartCoroutine("WoodchopTimer", hit);
        }
    }
    IEnumerator CollectionTimer(Collider other)
    {
        yield return new WaitForSeconds(1f);
        other.gameObject.SetActive(false); //hide carrot or kill agent being hit
        SetAnimatorBool(default);
        yield return new WaitForSeconds(1f);
        agentNavScript.DestinationCheck();
    }
    IEnumerator WoodchopTimer(IHealth hit)
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
            hit.TakeDamage(3f);
        }
        //agentNavScript.DestinationCheck();
    }
}