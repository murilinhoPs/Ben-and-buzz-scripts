using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    public static bool gamePaused = false;

    bool firstPause = true;

    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject controlsMenu;
    Human human;

    // Start is called before the first frame update
    void Awake()
    {
        gamePaused = false;
        human = FindObjectOfType<Human>();
    }

    // Update is called once per frame
    void Update()
    {

        if (InputManager.GetKeyDown("Pause") && !human.isPlayingMinigame)
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (gamePaused)
        {
            Time.timeScale = 1;
            gamePaused = false;
            controlsMenu.SetActive(false);
            optionsMenu.SetActive(false);
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 0;
            gamePaused = true;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
