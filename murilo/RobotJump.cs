using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotJump : MonoBehaviour
{
    private Vector3 offMeshLinkPos;
    public GameObject interactKey;
    public bool canJump;

    private void Update()
    {
        if (canJump)
        {
            if (!SwapPlayerCharacter.Instance.isHuman)
                interactKey.SetActive(true);
            else interactKey.SetActive(false);
        }
    }

    public void VerifyJump(NavMeshAgent agent, AnimationManager animation)
    {
        if (canJump && InputManager.GetKeyDown("Jump"))
        {
            DistanceManager.Instance.canMoveRobot = false;

            interactKey.SetActive(false);

            animation.SetBool("jumping", true);

            agent.SetDestination(offMeshLinkPos);

        }

        if (Vector3.Distance(transform.position, offMeshLinkPos) <= 1)
        {
            animation.SetBool("jumping", false);
            StartCoroutine(SetJumpToFalse(agent));
        }
    }

    public IEnumerator SetJumpToFalse(NavMeshAgent agent)
    {
        yield return new WaitForSeconds(0.3f);

        agent.enabled = false;

        yield return new WaitForSeconds(0.3f);

        agent.enabled = true;

        DistanceManager.Instance.canMoveRobot = true;

        canJump = false;
    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("jump"))
        {
            if (!SwapPlayerCharacter.Instance.isHuman)
                interactKey.SetActive(true);

            canJump = true;

            offMeshLinkPos = other.transform.parent.GetChild(1).transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("jump"))
        {
            canJump = false;
            interactKey.SetActive(false);
        }
    }
}
