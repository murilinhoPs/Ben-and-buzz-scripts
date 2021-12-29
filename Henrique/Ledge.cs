using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ledge : MonoBehaviour
{
    Transform robot;
    public bool falling;

    private void Start()
    {
        robot = GameObject.Find("Robot").transform;
    }

    private void Update()
    {
        while (falling)
        {
            if (robot.GetComponent<Rigidbody>().velocity.y == 0)
            {
                robot.parent.GetComponent<NavMeshAgent>().enabled = true;
                falling = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == robot.gameObject)
        {
            robot.parent.GetComponent<NavMeshAgent>().enabled = false;
            //robot.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 2.5f), ForceMode.Impulse);
            robot.parent.Translate(new Vector3(0, 0, 0.6408f));
            falling = true;
        }
    }
}
