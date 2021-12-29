using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manivela : MonoBehaviour
{

    public GameObject MCamera;
    public GameObject elevator;
    public Transform point;

    public GameObject interactInput;
    public GameObject robot;

    public bool control = false;

    private bool makeWork = false;

    private void Start()
    {
        robot = GameObject.Find("RobotWalker");
    }

    private void Update()
    {
        if (makeWork)
        {
            if (!SwapPlayerCharacter.Instance.isHuman)
            {
                interactInput.SetActive(true);

                if (InputManager.GetKeyDown("Interact"))
                {
                    //Debug.Log("elevador");
                    //elevator.GetComponent<Elevator>().up = !elevator.GetComponent<Elevator>().up;
                    //elevator.GetComponent<Elevator>().down = !elevator.GetComponent<Elevator>().down;

                    //interactInput.SetActive(false);
                    // DistanceManager.Instance.canMoveRobot = !DistanceManager.Instance.canMoveRobot;
                    control = !control;
                    if (!control)
                    {
                        robot.transform.parent = null;
                    }
                }

                if (control)
                {
                    MCamera.SetActive(true);
                    robot.GetComponentInChildren<Robot>().EmpurraParado();
                    robot.transform.parent = transform;
                    robot.transform.position = point.position;
                    robot.transform.Find("Buzz").transform.rotation = point.rotation;
                    if (InputManager.GetKey("Backward"))
                    {
                        if (elevator.name == "Elevador")
                        {
                            elevator.GetComponent<Elevator>().up = true;
                            transform.Rotate(0, 1.3f, 0);
                            robot.GetComponentInChildren<Robot>().EmpurraAndando();
                            //if (elevator.transform.position.y < elevator.GetComponent<Elevator>().highestPoint)
                            //{
                            //    transform.Rotate(0, 1.3f, 0);
                            //}
                        }
                        if (elevator.name == "MovingPlatform")
                        {
                            elevator.GetComponent<MovingPlatform>().front = true;
                            transform.Rotate(0, 1.3f, 0);
                            robot.GetComponentInChildren<Robot>().EmpurraAndando();
                            //if (elevator.transform.position.x < elevator.GetComponent<MovingPlatform>().highestPoint.position.x)
                            //{
                            //    transform.Rotate(0, 1.3f, 0);
                            //}
                        }
                    }
                    else
                    {
                        if (elevator.name == "Elevador")
                            elevator.GetComponent<Elevator>().up = false;
                        else if (elevator.name == "MovingPlatform")
                            elevator.GetComponent<MovingPlatform>().front = false;
                    }
                    if (InputManager.GetKey("Forward"))
                    {
                        if (elevator.name == "Elevador")
                        {
                            elevator.GetComponent<Elevator>().down = true;
                            transform.Rotate(0, -1.3f, 0);
                            robot.GetComponentInChildren<Robot>().EmpurraAndando();
                            //if (elevator.transform.position.y > elevator.GetComponent<Elevator>().lowestPoint)
                            //{
                            //    transform.Rotate(0, -1.3f, 0);
                            //}
                        }
                        if (elevator.name == "MovingPlatform")
                        {
                            elevator.GetComponent<MovingPlatform>().back = true;
                            transform.Rotate(0, -1.3f, 0);
                            robot.GetComponentInChildren<Robot>().EmpurraAndando();
                            //if (elevator.transform.position.x > elevator.GetComponent<MovingPlatform>().lowestPoint.position.x)
                            //{
                            //    transform.Rotate(0, -1.3f, 0);
                            //}
                        }
                    }
                    else
                    {
                        if (elevator.name == "Elevador")
                            elevator.GetComponent<Elevator>().down = false;
                        else if (elevator.name == "MovingPlatform")
                            elevator.GetComponent<MovingPlatform>().back = false;
                    }
                }
                else
                {
                    MCamera.SetActive(false);
                }
            }
            else
            {
                interactInput.SetActive(false);
                // DistanceManager.Instance.canMoveRobot = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Robot" && !SwapPlayerCharacter.Instance.isHuman)
        {
            interactInput.SetActive(true);

            makeWork = true;

            robot.GetComponentInChildren<Robot>().currentInteractState = InteractStates.HANDLE;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Robot")
        {
            interactInput.SetActive(false);

            makeWork = false;

            robot.GetComponentInChildren<Robot>().currentInteractState = InteractStates.NONE;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Robot" && !SwapPlayerCharacter.Instance.isHuman)
        {
            makeWork = true;

            robot.GetComponentInChildren<Robot>().currentInteractState = InteractStates.HANDLE;
        }
    }
}
