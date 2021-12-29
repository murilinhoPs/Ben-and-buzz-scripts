using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public Transform topSpawn;
    public Transform botSpawn;
    public GameObject playerContainer;
    private GameObject playerWithScript;
    public Transform elevator;
    public Transform placePlayer;

    public float highestPoint;
    public float lowestPoint;

    public bool auto = false;
    public int autoCD = 0;

    public bool up;
    public bool down;
    public float speed = 1;

    bool inside = false;
    public GameObject interact;

    private bool agentEnabled;

    void Update()
    {

        if (auto)
        {
            autoCD++;
            if (elevator.transform.position.y >= highestPoint)
            {
                up = false;
                autoCD++;
                if (autoCD > 300)
                {
                    down = true;
                    autoCD = 0;
                }
            }
            if (elevator.transform.position.y <= lowestPoint)
            {
                down = false;
                autoCD++;
                if (autoCD > 300)
                {
                    up = true;
                    autoCD = 0;
                }
            }
        }
        //movimento do elevador
        if (up && elevator.transform.position.y < highestPoint)
        {
            elevator.transform.position = new Vector3(elevator.transform.position.x, elevator.transform.position.y + speed * Time.deltaTime, elevator.transform.position.z);
        }
        if (down && elevator.transform.position.y > lowestPoint)
        {
            elevator.transform.position = new Vector3(elevator.transform.position.x, elevator.transform.position.y - speed * Time.deltaTime, elevator.transform.position.z);
        }

        if (inside)
        {

            if ((IsHuman() && !SwapPlayerCharacter.Instance.isHuman) ||
            (!IsHuman() && SwapPlayerCharacter.Instance.isHuman))
            {
                Deactivate();
                return;
            }

            if ((IsHuman() && SwapPlayerCharacter.Instance.isHuman) ||
            (!IsHuman() && !SwapPlayerCharacter.Instance.isHuman))
            {
                Activate();
            }

            if (InputManager.GetKeyDown("Interact"))//&& SwapPlayerCharacter.Instance.isHuman == true)
            {
                if (IsHuman())
                    agentEnabled = playerWithScript.GetComponent<Human>().agent.enabled;
                else
                    agentEnabled = playerWithScript.GetComponent<Robot>().agent.enabled;

                if (FindObjectOfType<Robot>().currentInteractState == InteractStates.NONE ||
                FindObjectOfType<Robot>().currentInteractState == InteractStates.PLATFORM ||
                FindObjectOfType<Robot>().currentInteractState == InteractStates.HANDLE)
                {
                    if (agentEnabled)
                    {
                        playerContainer.transform.position = playerContainer.transform.position = placePlayer.position;//new Vector3(elevator.transform.position.x, elevator.transform.position.y + 4.53f, elevator.transform.position.z);
                        playerContainer.transform.parent = transform;
                    }
                    else
                    {
                        if (elevator.transform.position.y > highestPoint) playerContainer.transform.position = topSpawn.position;
                        if (elevator.transform.position.y < lowestPoint) playerContainer.transform.position = botSpawn.position;
                        playerContainer.transform.parent = null;
                    }

                    playerWithScript.SendMessage("ToggleNavMesh");
                }
            }
        }
    }

    private bool IsHuman()
    {
        if (playerContainer.name == "Human")
            return true;
        else return false;
    }

    private void Activate()
    {
        if (playerContainer != null)
        {
            interact.SetActive(true);

            inside = true;
        }
    }

    private void Deactivate()
    {
        interact.SetActive(false);

        if (!IsHuman())
            playerWithScript.GetComponent<Robot>().currentInteractState = InteractStates.NONE;

        playerContainer = null;
        inside = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Human" || other.tag == "Robot")
        {
            playerWithScript = other.gameObject;
            playerContainer = other.transform.parent.gameObject;
            inside = true;
            interact.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Human" || other.tag == "Robot")
        {

            playerWithScript = other.gameObject;
            playerContainer = other.transform.parent.gameObject;
            inside = true;
            interact.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Robot" && !IsHuman())
        {
            playerWithScript.GetComponent<Robot>().currentInteractState = InteractStates.NONE;
        }

        interact.SetActive(false);
        playerContainer = null;
        playerWithScript = null;
        inside = false;
    }
}
