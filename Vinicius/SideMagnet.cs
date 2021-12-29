using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SideMagnet : MonoBehaviour
{
    public GameObject MCamera;
    public bool control = false;
    public bool active = false;
    public float speed = 1f;
    public bool right;
    public bool left;
    public bool up;
    public bool down;

    [SerializeField] private Transform robotObject;
    [Tooltip("Apenas para referência se pegou a cabeça ou não. Não mexer")]
    [SerializeField] private Transform headObject;
    [Tooltip("compensação do tamanho do imã para o robô não entrar no imã")]
    public float difference;

    public bool xAxisMovement = true;
    public bool invert = false;

    void Update()
    {

        if (control)
        {
            MCamera.SetActive(true);

            if (up)
            {
                if (InputManager.GetKey("MagnetUp"))
                {
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
                    }
                }
            }
            if (down)
            {
                if (InputManager.GetKey("MagnetDown"))
                {
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
                    }
                }
            }
            if (left)
            {
                if (InputManager.GetKey("MagnetLeft"))
                {
                    if (invert)
                    {
                        if (xAxisMovement)
                        {
                            transform.position = new Vector3(transform.position.x - speed * -1 * Time.deltaTime, transform.position.y, transform.position.z);
                        }
                        else
                            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * -1 * Time.deltaTime);
                    }
                    else
                    {
                        if (xAxisMovement)
                        {
                            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
                        }
                        else
                            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
                    }
                }
            }
            if (right)
            {
                if (InputManager.GetKey("MagnetRight"))
                {
                    if (invert)
                    {
                        if (xAxisMovement)
                        {
                            transform.position = new Vector3(transform.position.x + speed * -1 * Time.deltaTime, transform.position.y, transform.position.z);
                        }
                        else
                            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * -1 * Time.deltaTime);
                    }
                    else
                    {
                        if (xAxisMovement)
                        {
                            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
                        }
                        else
                            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);
                    }
                }
            }

            //--------------------------------------------------------------------------------------------------------------------------------------------
            //if (xAxis && !yAxis)
            //{
            //    if (xAxisMovement)
            //        transform.position = new Vector3(transform.position.x - Input.GetAxis("Horizontal") * velocity * Time.deltaTime,
            //        transform.position.y, transform.position.z);
            //    else
            //        transform.position = new Vector3(transform.position.x, transform.position.y,
            //                             transform.position.z - Input.GetAxis("Horizontal") * velocity * Time.deltaTime);
            //}
            //else if (yAxis && !xAxis)
            //{
            //    transform.position = new Vector3(transform.position.x, transform.position.y + Input.GetAxis("Vertical") * velocity * Time.deltaTime, transform.position.z);
            //}
            //else if (xAxis && yAxis)
            //{
            //    if (xAxisMovement)
            //        transform.position = new Vector3(transform.position.x - Input.GetAxis("Horizontal") * velocity * Time.deltaTime,
            //        transform.position.y + Input.GetAxis("Vertical") * velocity * Time.deltaTime, transform.position.z);
            //    else
            //        transform.position = new Vector3(transform.position.x,
            //        transform.position.y + Input.GetAxis("Vertical") * velocity * Time.deltaTime,
            //        transform.position.z - Input.GetAxis("Horizontal") * velocity * Time.deltaTime);
            //}

            if (InputManager.GetKeyDown("Activate"))
            {
                active = !active;
            }

            if (active)
            {
                if (robotObject != null && headObject == null && !FindObjectOfType<Robot>().carryingWheelchair)
                {
                    print("Aqui");
                    robotObject.GetChild(0).SendMessage("DisableNavMesh");

                    SwapPlayerCharacter.Instance.canChange = false;

                    robotObject.transform.parent = transform;

                    if (xAxisMovement)
                    {
                        if (invert)
                        {
                            if (robotObject.transform.position.x < transform.position.x - difference)
                            {
                                robotObject.transform.position = new Vector3(robotObject.transform.position.x, robotObject.transform.position.y,
                                    robotObject.transform.position.z + 0.03f);
                            }
                        }
                        else
                        {
                            if (robotObject.transform.position.x > transform.position.x + difference)
                            {
                                robotObject.transform.position = new Vector3(robotObject.transform.position.x, robotObject.transform.position.y,
                                    robotObject.transform.position.z - 0.03f);
                            }
                        }
                    }
                    else
                    if (invert)
                    {
                        if (robotObject.transform.position.x < transform.position.x - difference)
                        {
                            robotObject.transform.position = new Vector3(robotObject.transform.position.x + 0.03f, robotObject.transform.position.y,
                            robotObject.transform.position.z);
                        }
                    }
                    else
                    {
                        if (robotObject.transform.position.x > transform.position.x + difference)
                        {
                            robotObject.transform.position = new Vector3(robotObject.transform.position.x - 0.03f, robotObject.transform.position.y,
                            robotObject.transform.position.z);
                        }
                    }
                }

                if (headObject != null && robotObject == null)
                {
                    headObject.GetComponent<NavMeshAgent>().enabled = false;

                    headObject.transform.parent = transform;

                    if (xAxisMovement)
                    {
                        if (invert)
                        {
                            if (headObject.transform.position.z > transform.position.z + difference)
                            {
                                headObject.transform.position = new Vector3(headObject.transform.position.x, headObject.transform.position.y,
                                headObject.transform.position.z - 0.03f);
                            }
                        }
                        else
                        {
                            if (headObject.transform.position.z < transform.position.z - difference)
                            {
                                headObject.transform.position = new Vector3(headObject.transform.position.x, headObject.transform.position.y,
                                headObject.transform.position.z + 0.03f);
                            }
                        }
                    }
                    else
                    if (invert)
                    {
                        if (headObject.transform.position.x < transform.position.x - difference)
                        {
                            headObject.transform.position = new Vector3(headObject.transform.position.x + 0.03f, headObject.transform.position.y,
                            headObject.transform.position.z);
                        }
                    }
                    else
                    {
                        if (headObject.transform.position.x > transform.position.x + difference)
                        {
                            headObject.transform.position = new Vector3(headObject.transform.position.x - 0.03f, headObject.transform.position.y,
                            headObject.transform.position.z);
                        }
                    }
                }
            }
            else if (!active)
            {
                // if (!FindObjectOfType<Robot>().carryingWheelchair)
                SwapPlayerCharacter.Instance.canChange = true;

                if (robotObject != null && headObject == null)
                {
                    robotObject.GetChild(0).SendMessage("EnableNavMesh");

                    robotObject.transform.parent = null;

                    print("RobotIma");
                }
                if (headObject != null && robotObject == null)
                {
                    headObject.GetComponent<NavMeshAgent>().enabled = true;

                    headObject.transform.parent = null;
                }
            }
        }
        else if (!control && !DialogueManager.inDialogue)
        {
            MCamera.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Cabeca")
        {
            headObject = other.transform;
        }

        if (other.gameObject.tag == "Robot")
        {
            robotObject = other.transform.parent.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Robot" || other.gameObject.tag == "Cabeca"))
        {
            robotObject = null;
            headObject = null;
        }
    }
}
