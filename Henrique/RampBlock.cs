using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RampBlock : MonoBehaviour
{
    Ramp rampParent;
    NavMeshObstacle obs;

    public bool rampDialogue = true;
    Robot robot;
    Human human;


    private void Start()
    {
        rampParent = transform.parent.GetComponent<Ramp>();
        obs = GetComponent<NavMeshObstacle>();
        robot = FindObjectOfType<Robot>();
        human = FindObjectOfType<Human>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot"))
        {
            obs.enabled = false;
            return;
        }

        if (other.CompareTag("Human") && !rampParent.isRamp)
        {
            if (rampDialogue)
                GetComponent<PlayerTriggerDialogue>().dialogues[0].TriggerDialogue();

            obs.enabled = true;
        }

        if (other.CompareTag("Human") && gameObject.name == "Obstacle2" && !human.isDashing && rampParent.isRamp
        && !human.canPassObstacle2)
        {
            obs.enabled = true;
        }

        if (other.CompareTag("Human") && robot.carryingWheelchair && rampParent.isRamp)
            obs.enabled = false;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Robot"))
        {
            obs.enabled = false;
            return;
        }

        if (other.CompareTag("Human") && !rampParent.isRamp)
            obs.enabled = true;


        if (other.CompareTag("Human") && gameObject.name == "Obstacle2" && !human.isDashing && rampParent.isRamp
        && !human.canPassObstacle2)
        {
            obs.enabled = true;
        }

        if (other.CompareTag("Human") && robot.carryingWheelchair && rampParent.isRamp)
            obs.enabled = false;

    }

    private void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("Human"))
        obs.enabled = false;

        if (other.CompareTag("Human") && gameObject.name == "Obstacle2")
            human.canPassObstacle2 = false;
    }
}
