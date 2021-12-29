using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DistanceManager : MonoBehaviour
{
    public Transform player;

    public Transform robot;

    public bool canMoveRobot;
    public bool canMovePlayer;
    [HideInInspector] public bool canFix;

    [Tooltip("The min distance you need to be to use Ambu or fix the robot")]
    public float fixDistance = 5f;

    private float distBenBuzz;

    private static DistanceManager _instance;
    public static DistanceManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DistanceManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("DistanceManager");
                    _instance = container.AddComponent<DistanceManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        canMovePlayer = false;
        canMoveRobot = false;
        canFix = false;
    }

    void Update()
    {
        distBenBuzz = Vector3.Distance(player.position, robot.position);

        // Uncomment this line to see the distance between the characters
        // print("Distance between player and robot: " + dist);

        if (distBenBuzz <= fixDistance)
            canFix = true;
        else canFix = false;

        if (SwapPlayerCharacter.Instance.isHuman)
        {
            if (Input.GetKeyDown(KeyCode.T) && Cheat.Instance.cheatActivated)
            {
                robot.GetComponent<NavMeshAgent>().enabled = false;

                robot.transform.localPosition = new Vector3(player.transform.position.x + 1f, player.transform.position.y, player.transform.position.z);
            }
            if (distBenBuzz <= 1)
                robot.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
