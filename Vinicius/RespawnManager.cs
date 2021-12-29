using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static GameObject instance;
    public Transform[] humanRespawn;
    public Transform[] robotRespawn;
    public static int respawnArea = 0;

    public GameObject human;
    public GameObject robot;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
        human = GameObject.Find("Human");
        robot = GameObject.Find("RobotWalker");
    }

    public void Respawn()
    {
        SceneManager.LoadScene("J2 - Cenário");
        human.transform.position = humanRespawn[respawnArea].position;
        robot.transform.position = robotRespawn[respawnArea].position;

        SwapPlayerCharacter.Instance.isHuman = true;
    }
}
