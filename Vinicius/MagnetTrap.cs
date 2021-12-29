using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagnetTrap : MonoBehaviour
{
    [HideInInspector] public Transform Robot;
    public bool active;
    public Transform grabPos;

    public bool hasEntered;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hasEntered)
        {
            if (active)
            {
                Robot.GetComponent<NavMeshAgent>().enabled = false;
                Robot.transform.parent = transform;
                Robot.transform.position = grabPos.position;
            }
            else
            {
                Robot.GetComponent<NavMeshAgent>().enabled = true;
                Robot.transform.parent = null;

                hasEntered = false;


            }
        }
    }

    public void Deactivate()
    {
        active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Robot")
        {
            hasEntered = true;
            Robot = other.transform.parent.transform;

            SwapPlayerCharacter.Instance.isHuman = true;
        }
    }
}
