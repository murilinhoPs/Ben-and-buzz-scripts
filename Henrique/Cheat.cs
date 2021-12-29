using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Cheat : MonoBehaviour
{
    [HideInInspector] public bool cheatActivated = false;
    public bool canCheat = true;

    Transform humano, robo;
    public Transform h1, h2, h3, h4, h5, h6, r1, r2, r3, r4, r5, r6;

    private static Cheat _instance;
    public static Cheat Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Cheat>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("Cheats");
                    _instance = container.AddComponent<Cheat>();
                }
            }

            return _instance;
        }
    }

    private void Start()
    {
        humano = GameObject.Find("Human").transform;
        robo = GameObject.Find("RobotWalker").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && canCheat) cheatActivated = true;

        if (cheatActivated)
        {
            TeleportCheat();
            AdjustPos();
            Fase2();
        }
    }

    void Fase2()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene("Cenario - Fase 2");
        }
    }

    void AdjustPos()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            humano.parent = null;
            robo.parent = null;
            GameObject.Find("WheelchairPos").transform.position = new Vector3(0f, 0f, 0f);
            robo.GetComponent<Robot>().carryingWheelchair = false;
        }
    }

    void TeleportCheat()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            humano.GetComponent<NavMeshAgent>().enabled = false;
            robo.GetComponent<NavMeshAgent>().enabled = false;
            humano.position = h1.position;
            robo.position = r1.position;
            humano.GetComponent<NavMeshAgent>().enabled = true;
            robo.GetComponent<NavMeshAgent>().enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            humano.GetComponent<NavMeshAgent>().enabled = false;
            robo.GetComponent<NavMeshAgent>().enabled = false;
            humano.position = h2.position;
            robo.position = r2.position;
            humano.GetComponent<NavMeshAgent>().enabled = true;
            robo.GetComponent<NavMeshAgent>().enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            humano.GetComponent<NavMeshAgent>().enabled = false;
            robo.GetComponent<NavMeshAgent>().enabled = false;
            humano.position = h3.position;
            robo.position = r3.position;
            humano.GetComponent<NavMeshAgent>().enabled = true;
            robo.GetComponent<NavMeshAgent>().enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            humano.GetComponent<NavMeshAgent>().enabled = false;
            robo.GetComponent<NavMeshAgent>().enabled = false;
            humano.position = h4.position;
            robo.position = r4.position;
            humano.GetComponent<NavMeshAgent>().enabled = true;
            robo.GetComponent<NavMeshAgent>().enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            humano.GetComponent<NavMeshAgent>().enabled = false;
            robo.GetComponent<NavMeshAgent>().enabled = false;
            humano.position = h5.position;
            robo.position = r5.position;
            humano.GetComponent<NavMeshAgent>().enabled = true;
            robo.GetComponent<NavMeshAgent>().enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            humano.GetComponent<NavMeshAgent>().enabled = false;
            robo.GetComponent<NavMeshAgent>().enabled = false;
            humano.position = h6.position;
            robo.position = r6.position;
            humano.GetComponent<NavMeshAgent>().enabled = true;
            robo.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}