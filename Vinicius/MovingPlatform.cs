using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Transform frontSpawn;
    public Transform backSpawn;
    public GameObject playerContainer;
    private GameObject playerWithScript;
    public Transform platform;
    public Transform highestPoint;
    public Transform lowestPoint;
    public Transform placePlayer;
    public bool front;
    public bool back;
    public bool automatic;
    private bool moveRight;
    public bool xAxisMovement;


    public float speed = 1;
    bool inside = false;
    public GameObject interact;

    private bool agentEnabled;

    void Update()
    {

        if (xAxisMovement) moveOnX();
        else MoveOnZ();


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
                {
                    playerWithScript.GetComponent<Robot>().currentInteractState = InteractStates.PLATFORM;
                    agentEnabled = playerWithScript.GetComponent<Robot>().agent.enabled;
                }

                if (FindObjectOfType<Robot>().currentInteractState == InteractStates.NONE ||
                FindObjectOfType<Robot>().currentInteractState == InteractStates.PLATFORM ||
                FindObjectOfType<Robot>().currentInteractState == InteractStates.HANDLE)
                {
                    if (agentEnabled)
                    {
                        playerContainer.transform.position = placePlayer.position;//new Vector3(platform.transform.position.x, platform.transform.position.y + 1.2f, platform.transform.position.z);
                        playerContainer.transform.parent = transform;
                    }
                    else
                    {
                        if (xAxisMovement)
                        {
                            if (platform.transform.position.x >= highestPoint.position.x) playerContainer.transform.position = frontSpawn.position;
                            if (platform.transform.position.x <= lowestPoint.position.x) playerContainer.transform.position = backSpawn.position;
                            playerContainer.transform.parent = null;
                        }
                        if (!xAxisMovement)
                        {
                            if (platform.transform.position.z >= highestPoint.position.z) playerContainer.transform.position = frontSpawn.position;
                            if (platform.transform.position.z <= lowestPoint.position.z) playerContainer.transform.position = backSpawn.position;
                            playerContainer.transform.parent = null;
                        }
                    }

                    playerWithScript.SendMessage("ToggleNavMesh");
                }
            }
        }
    }

    private void moveOnX()
    {
        if (automatic)
        {
            if (platform.transform.position.x <= lowestPoint.position.x)
                moveRight = true;
            else if (platform.transform.position.x >= highestPoint.position.x)
                moveRight = false;

            if (moveRight)
            {
                platform.transform.position = new Vector3(platform.transform.position.x + speed * Time.deltaTime,
                platform.transform.position.y, platform.transform.position.z);
            }
            else
            {
                platform.transform.position = new Vector3(platform.transform.position.x - speed * Time.deltaTime,
                platform.transform.position.y, platform.transform.position.z);
            }
        }
        else
        {
            //movimento do elevador
            if (front && platform.transform.position.x <= highestPoint.position.x)
            {
                platform.transform.position = new Vector3(platform.transform.position.x + speed * Time.deltaTime,
                platform.transform.position.y, platform.transform.position.z);
            }
            if (back && platform.transform.position.x >= lowestPoint.position.x)
            {
                platform.transform.position = new Vector3(platform.transform.position.x - speed * Time.deltaTime,
                 platform.transform.position.y, platform.transform.position.z);
            }
        }
    }

    private void MoveOnZ()
    {
        if (automatic)
        {
            if (platform.transform.position.z <= lowestPoint.position.z)
                moveRight = true;
            else if (platform.transform.position.z >= highestPoint.position.z)
                moveRight = false;

            if (moveRight)
            {
                platform.transform.position = new Vector3(platform.transform.position.x,
                platform.transform.position.y, platform.transform.position.z + speed * Time.deltaTime);
            }
            else
            {
                platform.transform.position = new Vector3(platform.transform.position.x,
                platform.transform.position.y, platform.transform.position.z - speed * Time.deltaTime);
            }
        }
        else
        {
            //movimento do elevador
            if (front && platform.transform.position.z <= highestPoint.position.z)
            {
                platform.transform.position = new Vector3(platform.transform.position.x,
                platform.transform.position.y, platform.transform.position.z + speed * Time.deltaTime);
            }
            if (back && platform.transform.position.z >= lowestPoint.position.z)
            {
                platform.transform.position = new Vector3(platform.transform.position.x,
                 platform.transform.position.y, platform.transform.position.z - speed * Time.deltaTime);
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
