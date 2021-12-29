using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapPlayerCharacter : MonoBehaviour
{
    [HideInInspector] public bool isHuman;
    [HideInInspector] public bool carrying;

    public GameObject playerHud;
    public GameObject robotHud;

    [HideInInspector] public bool magnetControl;
    public bool canChange = false;
    public static SwapPlayerCharacter _instance;
    public static SwapPlayerCharacter Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SwapPlayerCharacter>();
            }

            return _instance;
        }
        set
        {

        }
    } // Criei um singleton dessa classe. É o player manager, tem que ser global

    void Start()
    {
        isHuman = true;

        robotHud.SetActive(false);
        playerHud.SetActive(true);

        CameraFocus.activeCamera = "Human";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && Cheat.Instance.cheatActivated) canChange = true;

        //Assegura q não é possível mudar de controle quando o robô estiver levando o inventor
        if (!isHuman && carrying) return;
        if (magnetControl) return;

        if (canChange)
        {
            if (InputManager.GetKeyDown("Swap Character"))
            {
                if (isHuman)
                {
                    //Passa o controle pro robô
                    isHuman = false;

                    robotHud.SetActive(true);
                    playerHud.SetActive(false);

                    CameraFocus.activeCamera = "Robot";

                }
                else
                {
                    //Passa o controle pro inventor
                    isHuman = true;

                    robotHud.SetActive(false);
                    playerHud.SetActive(true);

                    CameraFocus.activeCamera = "Human";
                }
            }
        }
    }
}
