using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minigame : MonoBehaviour
{
    public enum Comportamento { ROBOT_FIX, DOOR, RAMP, STAIR, MAGNET_FIX, GRADE, ELEVADOR_AUTO }
    public Comportamento currentBehaviour;
    public GameObject solutionTarget, solutionTarget2, interact;
    bool activated, setUp;
    public AudioClip success;

    public GameObject meuMinigame, gameCanvas;

    [SerializeField] private bool triggerDoorDialog = true;

    Human human;

    private void Awake()
    {
        meuMinigame.transform.GetChild(0).gameObject.GetComponentInChildren<MinigameGearSpin>().minigame = this;
        human = FindObjectOfType<Human>();
    }

    private void Update()
    {
        GetComponent<AudioSource>().volume = OptionsManager.effectVolume;
        if (setUp && !activated && Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine("Action");
        }

        if (setUp && !activated && InputManager.GetKeyDown("Interact"))
        {
            meuMinigame.SetActive(true);
            meuMinigame.transform.GetChild(0).gameObject.GetComponentInChildren<MinigameGearSpin>().SetUp();
            DistanceManager.Instance.canMovePlayer = false;
            gameCanvas.SetActive(false);
            human.isPlayingMinigame = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Human") && !activated)
        {
            setUp = true;
            interact.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Human") && !activated)
        {
            setUp = false;
            interact.SetActive(false);
        }
    }

    public void QuitMinigame()
    {
        setUp = false;
    }

    public IEnumerator Action()
    {
        yield return null;

        activated = true;
        interact.SetActive(false);
        GetComponent<AudioSource>().PlayOneShot(success);

        DistanceManager.Instance.canMovePlayer = true;

        switch (currentBehaviour)
        {
            case Comportamento.DOOR:
                solutionTarget.SetActive(false);
                // solutionTarget.GetComponent<MeshRenderer>().enabled = false;
                // //TODO: Animacao porta
                // solutionTarget.GetComponent<NavMeshObstacle>().enabled = false;

                if (triggerDoorDialog)
                    GetComponent<PlayerTriggerDialogue>().dialogues[0].TriggerDialogue();

                break;
            case Comportamento.RAMP:
                solutionTarget.GetComponent<Ramp>().isRamp = true;
                if (solutionTarget2) solutionTarget2.GetComponent<Ramp>().isRamp = true;
                break;
            case Comportamento.ROBOT_FIX:

                break;
            case Comportamento.MAGNET_FIX:
                solutionTarget.GetComponent<MagnetPanel>().broken = false;
                break;
            case Comportamento.STAIR:
                solutionTarget.GetComponent<Ramp>().isRamp = true;
                break;
            case Comportamento.GRADE:
                solutionTarget.GetComponent<Animator>().SetBool("gradeDown", true);
                solutionTarget.GetComponent<NavMeshObstacle>().enabled = false;
                break;
            case Comportamento.ELEVADOR_AUTO:
                solutionTarget.GetComponent<MovingPlatform>().automatic = true;
                break;
            default:
                break;
        }

        print("terminou o minigame");
        GetComponent<Animator>().Play("Working");

        this.enabled = false;
    }
}
