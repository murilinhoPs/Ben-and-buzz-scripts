using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotNavMesh : MonoBehaviour
{
    NavMeshAgent agent;
    Transform guide;
    Vector3 walkDirection;
    public float speed = 10.0f;
    Transform ttt;

    NavMeshHit nav;
    public float turnSmoothing = 15f;

    void Start()
    {
        agent = transform.parent.GetComponent<NavMeshAgent>();
        guide = GameObject.Find("WalkGuide").transform;
        ttt = transform;
    }

    void Update()
    {
        Walk();
    }

    void Walk()
    {
        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            agent.isStopped = false;
            walkDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            walkDirection = guide.TransformDirection(walkDirection * (-1f));
            agent.SetDestination(ttt.position + walkDirection);

            Quaternion targetRotation = Quaternion.LookRotation(walkDirection, Vector3.up);
            Quaternion newRotation = Quaternion.Lerp(ttt.rotation, targetRotation, turnSmoothing * Time.deltaTime);
            ttt.rotation = newRotation;
        }
        else
        {
            agent.isStopped = true;
        }
    }
}
