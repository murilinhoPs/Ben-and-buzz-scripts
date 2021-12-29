using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeuMinigame : MonoBehaviour
{
    public GameObject[] children;

    public int completeSteps;

    public int step = 0;

    public GameObject puzzleCamera;

    public GameObject gameCanvas;

    public Minigame minigame;

    void Start()
    {
        completeSteps = children.Length;
    }

    private void Update()
    {
        if (step >= completeSteps)
        {
            print("Puzzle Completo!");

            StartCoroutine(backToGame());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            stopMinigame();
            minigame.QuitMinigame();
        }
    }

    void stopMinigame()
    {
        puzzleCamera.SetActive(false);
        gameCanvas.SetActive(true);

        // DistanceManager.Instance.canMovePlayer = true;
    }

    IEnumerator backToGame()
    {
        StartCoroutine(minigame.Action());

        yield return new WaitForSeconds(2f);

        puzzleCamera.SetActive(false);
        gameCanvas.SetActive(true);

        // DistanceManager.Instance.canMovePlayer = true;

    }
}
