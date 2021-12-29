using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameGearSpin : MonoBehaviour
{
    [HideInInspector] public Minigame minigame;
    public GameObject tutorialScreen;
    [HideInInspector] public GameObject startingGear;
    [HideInInspector] public GameObject[] endingGears;
    [HideInInspector] public GameObject[] mobileGears;
    [HideInInspector] public GameObject[] pistons;

    [HideInInspector] public MinigamePiston activePiston;

    [HideInInspector] public List<GameObject> path;
    [HideInInspector] public List<GameObject> adjacentToStart;

    [HideInInspector] public List<GameObject> innerParts;
    [HideInInspector] public List<GameObject> outerParts;

    bool hasWon;

    private void Start()
    {
        startingGear = transform.parent.Find("EngrenagemInicial").gameObject;
        endingGears = new GameObject[transform.parent.Find("EngrenagensFinais").childCount];
        for (int i = 0; i < endingGears.Length; i++)
        {
            endingGears[i] = transform.parent.Find("EngrenagensFinais").GetChild(i).gameObject;
        }
        mobileGears = new GameObject[transform.parent.Find("EngrenagensMoveis").childCount];
        for (int i = 0; i < mobileGears.Length; i++)
        {
            mobileGears[i] = transform.parent.Find("EngrenagensMoveis").GetChild(i).gameObject;
        }
        pistons = new GameObject[transform.parent.Find("Pistons").childCount];
        for (int i = 0; i < pistons.Length; i++)
        {
            pistons[i] = transform.parent.Find("Pistons").GetChild(i).gameObject;
        }

        outerParts.Add(startingGear.transform.Find("OuterPart").gameObject);
        innerParts.Add(startingGear.transform.Find("InnerPart").gameObject);
        foreach (GameObject item in endingGears)
        {
            outerParts.Add(item.transform.Find("OuterPart").gameObject);
            innerParts.Add(item.transform.Find("InnerPart").gameObject);
        }
        foreach (GameObject item in mobileGears)
        {
            outerParts.Add(item.transform.Find("OuterPart").gameObject);
            innerParts.Add(item.transform.Find("InnerPart").gameObject);
        }

        startingGear.GetComponentInChildren<GearOuterPart>().minigameMasterControl = this;
        startingGear.GetComponentInChildren<MinigameFixedGears>().minigameMasterControl = this;
        foreach (GameObject item in endingGears)
        {
            item.GetComponentInChildren<GearOuterPart>().minigameMasterControl = this;
            item.GetComponentInChildren<MinigameFixedGears>().minigameMasterControl = this;
        }
        foreach (GameObject item in mobileGears)
        {
            item.GetComponentInChildren<GearOuterPart>().minigameMasterControl = this;
            item.GetComponent<MinigameMobileGears>().minigameMasterControl = this;
        }
        if (pistons.Length != 0)
            foreach (GameObject item in pistons)
            {
                item.GetComponent<MinigamePiston>().minigameMasterControl = this;
            }

        for (int i = 0; i < mobileGears.Length; i++)
        {
            float x = ((float)i + 1f) / ((float)mobileGears.Length + 1f);
            float y = Mathf.Lerp(-515.626f, 515.625f, x);
            mobileGears[i].transform.localPosition = new Vector3(y, 0, 0);
        }

        transform.parent.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        CheckPath();
        CheckVictory();

        if (tutorialScreen.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                tutorialScreen.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Escape))
                tutorialScreen.transform.parent.transform.parent.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform.parent.transform.parent.gameObject.SetActive(false);
            minigame.gameCanvas.SetActive(true);

            Cursor.visible = true;

            Cursor.lockState = CursorLockMode.Locked;
            DistanceManager.Instance.canMovePlayer = true;

            FindObjectOfType<Human>().isPlayingMinigame = false;
        }
    }

    public void ResetGears()
    {
        for (int i = 0; i < mobileGears.Length; i++)
        {
            float x = ((float)i + 1f) / ((float)mobileGears.Length + 1f);
            float y = Mathf.Lerp(-515.626f, 515.625f, x);
            mobileGears[i].transform.localPosition = new Vector3(y, 0, 0);
        }
        foreach (GameObject item in pistons)
        {
            item.GetComponent<MinigamePiston>().ResetPiston();
        }
    }

    void CheckPath()
    {
        bool eee = false;

        foreach (GameObject item in path)
        {
            if (!item.transform.Find("OuterPart").GetComponent<GearOuterPart>().spinning) eee = true;
        }

        if (eee) path.Clear();

        if (!path.Contains(startingGear)) path.Add(startingGear);
    }

    void CheckVictory()
    {
        bool vvv = true;
        foreach (GameObject item in endingGears)
        {
            if (!path.Contains(item)) vvv = false;
        }

        if (vvv && !hasWon) StartCoroutine(backToGame());
    }

    public void SetUp()
    {
        ResetGears();
        tutorialScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator backToGame()
    {
        hasWon = true;

        StartCoroutine(minigame.Action());

        yield return new WaitForSeconds(2f);


        transform.parent.parent.gameObject.SetActive(false);
        minigame.gameCanvas.SetActive(true);

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;
        DistanceManager.Instance.canMovePlayer = true;
        FindObjectOfType<Human>().isPlayingMinigame = false;
    }
}
