using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameGenius : MonoBehaviour
{
    public Image colorDisplayer;
    Sprite defaultDisplayer;

    public int sequenceLength = 4;
    public Sprite[] colors;
    public Sprite[] colorSequence;
    public Selectable[] colorButtons;

    public enum Phases { DISPLAY, INSERT, CHECKUP }
    public Phases currentPhase;

    public Image[] insertionIcons;
    [HideInInspector]
    public int currentInsertion;

    public Text result;

    public Minigame minigame;

    public GameObject puzzleCamera;

    public GameObject gameCanvas;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;


        defaultDisplayer = colorDisplayer.sprite;
        colorSequence = new Sprite[sequenceLength];

        for (int i = 0; i < colorSequence.Length; i++)
        {
            colorSequence[i] = colors[Random.Range(0, colors.Length)];
        }

        currentPhase = Phases.DISPLAY;
        StartCoroutine(SequenceDisplay());

        currentInsertion = 0;
    }

    private void Update()
    {
        switch (currentPhase)
        {
            case Phases.DISPLAY:
                foreach (Selectable item in colorButtons)
                {
                    item.interactable = false;
                }
                break;
            case Phases.INSERT:
                foreach (Selectable item in colorButtons)
                {
                    item.interactable = true;
                }
                break;
            case Phases.CHECKUP:
                foreach (Selectable item in colorButtons)
                {
                    item.interactable = false;
                }
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            stopMinigame();
            minigame.QuitMinigame();
        }
    }

    void stopMinigame()
    {
        Cursor.lockState = CursorLockMode.Locked;

        puzzleCamera.SetActive(false);
        gameCanvas.SetActive(true);

        // DistanceManager.Instance.canMovePlayer = true;
    }

    IEnumerator SequenceDisplay()
    {
        for (int i = 0; i < sequenceLength; i++)
        {
            yield return new WaitForSeconds(0.5f);
            colorDisplayer.sprite = colorSequence[i];
            yield return new WaitForSeconds(1f);
            colorDisplayer.sprite = defaultDisplayer;
        }

        Cursor.lockState = CursorLockMode.None;

        currentPhase = Phases.INSERT;
        StopCoroutine(SequenceDisplay());
    }

    public IEnumerator SequenceCheckup()
    {
        yield return new WaitForSeconds(0f);
        bool defeat = false;
        for (int i = 0; i < sequenceLength; i++)
        {
            if (insertionIcons[i].sprite != colorSequence[i])
            {
                defeat = true;
            }
        }

        if (!defeat)
        {
            result.gameObject.SetActive(true);
            result.text = "☺";
            StartCoroutine(backToGame());
        }
        else
        {
            result.gameObject.SetActive(true);
            result.text = "☹️";
            yield return new WaitForSeconds(1.5f);
            foreach (Image item in insertionIcons)
            {
                item.sprite = defaultDisplayer;
            }

            currentPhase = Phases.DISPLAY;
            currentInsertion = 0;
            result.gameObject.SetActive(false);
            result.text = " ";
            StartCoroutine(SequenceDisplay());
        }
    }

    IEnumerator backToGame()
    {
        StartCoroutine(minigame.Action());

        yield return new WaitForSeconds(2f);

        Cursor.lockState = CursorLockMode.Locked;

        puzzleCamera.SetActive(false);
        gameCanvas.SetActive(true);

        // DistanceManager.Instance.canMovePlayer = true;
    }
}
