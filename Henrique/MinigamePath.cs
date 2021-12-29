using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePath : MonoBehaviour
{
    public MinigamePathButton[] buttons;
    public List<MinigamePathButton> path;
    public List<MinigamePathButton> endingPoints;
    public GameObject vs;
    public GameObject tutorialScreen;

    public Minigame minigame;

    bool hasWon;

    private void Start()
    {
        foreach (MinigamePathButton item in buttons)
        {
            if (item.startingButton == MinigamePathButton.ButtonType.EXIT)
                endingPoints.Add(item);
        }

        //SetUp();
    }

    private void Update()
    {
        CheckPath();
        VictoryCheck();

        if (tutorialScreen.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                tutorialScreen.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform.parent.parent.gameObject.SetActive(false);
            minigame.gameCanvas.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            // DistanceManager.Instance.canMovePlayer = true;
        }
    }

    void CheckPath()
    {
        bool eee = false;

        foreach (MinigamePathButton item in path)
        {
            item.charged = true;
            if (item.nextToEntry) eee = true;
        }

        if (!eee) path.Clear();
    }

    void VictoryCheck()
    {
        bool vvv = true;
        foreach (MinigamePathButton item in endingPoints)
        {
            if (!item.charged) vvv = false;
        }

        if (vvv && !hasWon) StartCoroutine(backToGame());
    }

    public void ResetTiles()
    {
        foreach (MinigamePathButton item in buttons)
        {
            item.currentButton = item.startingButton;
            item.whiteAdjacent = null;
            item.charged = false;
            item.nextToEntry = false;
            item.nextToCharge = false;
        }
    }

    public void SetUp()
    {
        ResetTiles();
        tutorialScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator backToGame()
    {
        hasWon = true;

        StartCoroutine(minigame.Action());

        yield return new WaitForSeconds(2f);


        transform.parent.parent.gameObject.SetActive(false);
        minigame.gameCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        // DistanceManager.Instance.canMovePlayer = true;

        Debug.Log("A");
    }
}
