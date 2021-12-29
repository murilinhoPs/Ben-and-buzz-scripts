using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Human : MonoBehaviour
{
    Vector3 speedVector;

    private Robot robot;

    public Transform guide;
    //A velocidade de movimentação do inventor.
    public float speed = 5.0f;
    public float dashSpeed = 10.0f;
    //A velocidade de rotação do inventor.
    public float turnSmoothing = 150f;

    public NavMeshAgent agent;
    Vector3 walkDirection;
    [SerializeField] private bool fullStamina = true;

    private bool fullBreath = true;

    [SerializeField] private StaminaManager staminaManager;

    [SerializeField] private StaminaManager airManager;

    private PlayerTriggerDialogue playerDialogue;

    public Text remedyText;
    [Tooltip("A quantidade de pílulas")]
    public int remedyValue = 5;
    private bool resetStamina = true;

    public GameObject interactPlayer;

    private float totalTimer = 10.0f;
    private float currentTimer = 10.0f;
    private bool canTake = true;
    public GameObject timerCount;

    private SoundManager soundManager;
    public AnimationManager animationManager;
    public GameObject[] remedio;

    public Transform fixingRobotPos;

    bool canPlay = true;

    [HideInInspector] public bool firstCam = true;


    [SerializeField] private int firstEncountersDialogues = 0;

    public Rigidbody rigidBody;

    public bool isDashing;

    private CinemachineVirtualCamera cam;
    public bool canPassObstacle2;

    public bool isPlayingMinigame;

    public ParticleSystem dashSmoke;

    void Awake()
    {
        playerDialogue = GetComponent<PlayerTriggerDialogue>();
        soundManager = GetComponent<SoundManager>();

        SwapPlayerCharacter.Instance.isHuman = true;

        guide = GameObject.Find("HumanWalkGuide").transform;

        robot = FindObjectOfType<Robot>();
    }

    void Update()
    {
        if (!SwapPlayerCharacter.Instance.isHuman)
        {
            animationManager.SetBool("walking", false);
            staminaManager.restoreStamina(0.35f, false);
        }

        if (DistanceManager.Instance.canMovePlayer)
        {
            if (SwapPlayerCharacter.Instance.isHuman)
            {
                verifyStaminas();
                // fixingRobot();

                if (Cheat.Instance.cheatActivated)
                {
                    if (Input.GetKeyDown(KeyCode.O) && Cheat.Instance.cheatActivated)
                    {
                        staminaManager.restoreStamina(10f, true);
                        airManager.restoreStamina(10f, true);
                    }
                }
            }

            animationManager.GetBool("walking");

            if (fullBreath)
            {
                if (!fullStamina)
                {
                    staminaManager.restoreStamina(0.2f, false);

                    StartCoroutine(Fatigue());
                }

                if (agent.enabled)
                    verifyBreath();
            }
            else
                breathWarning();
        }
        else
        {
            animationManager.SetBool("walking", false);
        }

        if (isDashing)
            dashSmoke.Play();
        else
            dashSmoke.Stop();
    }

    void FixedUpdate()
    {
        if (DistanceManager.Instance.canMovePlayer && fullBreath && fullStamina && SwapPlayerCharacter.Instance.isHuman)
            Walk();
    }

    IEnumerator PerformDash(Vector3 direction)//NavMeshHit hit, Vector3 position
    {
        isDashing = true;

        agent.speed = 24f;
        agent.acceleration = 10f;

        agent.Move(direction * dashSpeed);

        yield return new WaitForSeconds(0.5f);

        yield return new WaitUntil(() => agent.transform.position.magnitude >= agent.nextPosition.magnitude);

        agent.isStopped = true;

        agent.speed = 4f;
        agent.acceleration = 8f;

        isDashing = false;
    }

    void Walk()
    {
        if (agent.isOnNavMesh)
        {
            if (GetAxisReserve("Horizontal") != 0f || GetAxisReserve("Vertical") != 0f)
            // if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
            {
                //fixedDeltaTime
                var horizontalInput = GetAxisReserve("Horizontal") * Time.fixedDeltaTime;
                var verticalInput = GetAxisReserve("Vertical") * Time.fixedDeltaTime;

                var direction = new Vector3(horizontalInput, 0f, verticalInput);
                walkDirection = guide.TransformDirection(direction * (-1f)) * speed;

                if (!isDashing)
                {
                    agent.Move(walkDirection);
                    // soundManager.Play("wheelchair");
                }

                if (InputManager.GetKeyDown("Dash") && SceneManager.GetActiveScene().name == "Cenario - Fase 2")
                {
                    // soundManager.Stop("wheelchair");

                    StartCoroutine(PerformDash(walkDirection));
                }


                Quaternion targetRotation = Quaternion.LookRotation(walkDirection, Vector3.up);
                // Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
                Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothing * Time.fixedDeltaTime);

                transform.rotation = newRotation;

                animationManager.SetBool("walking", true);

                airManager.takeAndResetStamina(fullBreath, 0.3f, resetStamina);
                staminaManager.takeAndResetStamina(fullStamina, 0.5f, resetStamina);

                canPlay = true;
            }
            else
            {
                animationManager.SetBool("walking", false);

                staminaManager.restoreStamina(0.35f, false);

                // soundManager.Stop("wheelchair");
            }
        }
    }

    void verifyStaminas()
    {

        remedyText.text = "X " + remedyValue.ToString();

        if (staminaManager.currentStamina <= 0)
        {
            fullStamina = false;
        }
        else if (staminaManager.currentStamina > 0)
        {
            fullStamina = true;

        }

        if (airManager.currentStamina <= 0)
        {
            fullBreath = false;
        }
        else
        {
            fullBreath = true;
        }
    }

    private void verifyBreath()
    {
        if (DistanceManager.Instance.canFix && !SwapPlayerCharacter.Instance.isHuman && SwapPlayerCharacter.Instance.canChange
        && !robot.GetComponent<Caixote>().isCarried)
        {
            print("CaixoteRobo -> " + robot.GetComponent<Caixote>().isCarried);

            if (airManager.currentStamina < airManager.totalStamina - 1)
            {
                if (!robot.canCarryWheelchair)
                    interactPlayer.SetActive(true);

                if (InputManager.GetKeyDown("Interact"))
                {
                    interactPlayer.SetActive(false);
                    playerDialogue.dialogues[0].TriggerDialogue();

                }

                if (playerDialogue.dialogues[0].dialogueManager.finished)
                {
                    StartCoroutine(FindObjectOfType<Robot>().Ambu());

                    //TODO: Animacao dele fazendo ambu
                    StartCoroutine(Ambu());
                }
            }
            else if (airManager.currentStamina >= airManager.totalStamina)
            {
                if (InputManager.GetKeyDown("Interact") && playerDialogue.dialogues[1].dialogueManager.sentences.Count == 0)
                {
                    interactPlayer.SetActive(false);
                    playerDialogue.dialogues[1].TriggerDialogue();
                }
            }

        }
        else
            interactPlayer.SetActive(false);

        if (airManager.currentStamina < airManager.totalStamina && !DistanceManager.Instance.canFix)
        {
            if (InputManager.GetKeyDown("Repair/Heal") && remedyValue > 0)
            {
                StartCoroutine(Pills());


                remedyValue--;
                airManager.takeAndResetStamina(false, 0f, resetStamina);
            }

        }
    }

    private void breathWarning()
    {
        if (canPlay)
        {
            animationManager.SetBool("noAir", true);
            playerDialogue.dialogues[3].TriggerDialogue();
            canPlay = false;
        }

        if (canTake)
            currentTimer -= 1 * Time.deltaTime;

        timerCount.SetActive(true);
        timerCount.GetComponent<Text>().text = String.Format("{0:0.}", currentTimer);

        if (currentTimer >= 9.8)
            soundManager.Play("warning");

        if (currentTimer <= 0)
            SceneManager.LoadScene("GameOver");

        if (currentTimer < totalTimer)
        {
            if (DistanceManager.Instance.canFix)
            {
                interactPlayer.SetActive(true);

                if (InputManager.GetKeyDown("Interact"))
                {
                    interactPlayer.SetActive(false);
                    playerDialogue.dialogues[2].TriggerDialogue();
                    canTake = false;

                    timerCount.transform.GetChild(0).gameObject.SetActive(false);
                }

                if (playerDialogue.dialogues[2].dialogueManager.finished == true)
                {
                    currentTimer = totalTimer;
                    timerCount.SetActive(false);
                    soundManager.Stop("warning");
                    canTake = true;
                    canPlay = true;

                    StartCoroutine(FindObjectOfType<Robot>().Ambu());

                    StartCoroutine(Ambu());
                }
            }
        }
    }

    private void fixingRobot()
    {
        if (robot.fixing && DistanceManager.Instance.canFix)
        {
            StartCoroutine(Fixing());
        }
    }

    private IEnumerator Fatigue()
    {
        fullStamina = true;
        staminaManager.restoreStamina(0.2f, false);

        animationManager.SetBool("fatigue", true);

        DistanceManager.Instance.canMovePlayer = false;

        playerDialogue.dialogues[4].TriggerDialogue();

        yield return new WaitForSeconds(4f);

        DistanceManager.Instance.canMovePlayer = true;
        animationManager.SetBool("fatigue", false);


    }

    private IEnumerator Pills()
    {
        remedio[0].SetActive(true);
        remedio[1].SetActive(true);

        animationManager.SetBool("takingPills", true);
        DistanceManager.Instance.canMovePlayer = false;

        soundManager.Play("pills");


        yield return new WaitForSeconds(3.5f);

        animationManager.SetBool("takingPills", false);
        DistanceManager.Instance.canMovePlayer = true;

        remedio[0].SetActive(false);
        remedio[1].SetActive(false);
    }

    private IEnumerator Ambu()
    {
        DistanceManager.Instance.canMovePlayer = false;

        animationManager.SetBool("noAir", false);
        animationManager.SetBool("doingAmbu", true);

        airManager.restoreStamina(0f, true);

        yield return new WaitForSeconds(1f);

        animationManager.SetBool("doingAmbu", false);
        DistanceManager.Instance.canMovePlayer = true;
    }

    public IEnumerator Fixing()
    {
        DistanceManager.Instance.canMovePlayer = false;
        DistanceManager.Instance.canMoveRobot = false;

        transform.rotation = Quaternion.LookRotation(fixingRobotPos.forward, Vector3.up);
        transform.parent.position = fixingRobotPos.position;

        yield return new WaitUntil(() => transform.parent.position == fixingRobotPos.position);

        animationManager.SetBool("fixing", true);

        yield return new WaitForSeconds(3.7f);

        animationManager.SetBool("fixing", false);

        robot.fixing = false;
        robot.fixParticle.SetActive(false);

        DistanceManager.Instance.canMovePlayer = true;
        DistanceManager.Instance.canMoveRobot = true;
    }

    private IEnumerator Catching()
    {
        DistanceManager.Instance.canMovePlayer = false;

        animationManager.SetBool("catching", true);

        yield return new WaitForSeconds(1.5f);

        animationManager.SetBool("catching", false);
        DistanceManager.Instance.canMovePlayer = true;
    }

    void OnTriggerEnter(Collider other)
    {

        CatchPills(other);

        Puzzles(other);

        Tutorials(other);

        Collectibles(other);

        if (other.gameObject.name == "Obstacle1") canPassObstacle2 = true;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("fase5Fim") && SceneManager.GetActiveScene().name == "Cenario - Fase 2")
        {

            if (playerDialogue.dialogues[22].dialogueManager.finished == true)
            {
                StartCoroutine(CutscenesCameras.Instance.activeOne("Puzzle5Fase2Fim", 25.0f));
                Destroy(other.gameObject);

            }
        }
        if (other.CompareTag("cantChange"))
        {
            SwapPlayerCharacter.Instance.canChange = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("cantChange"))
        {
            SwapPlayerCharacter.Instance.canChange = false;

            Destroy(other.gameObject);
        }
    }

    private void CatchPills(Collider other)
    {
        if (other.CompareTag("Fonte"))
        {
            remedyValue++;

            playerDialogue.dialogues[12].TriggerDialogue();

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Fonte1"))
        {
            remedyValue++;

            playerDialogue.dialogues[11].TriggerDialogue();

            Destroy(other.gameObject);
        }
    }

    private void Puzzles(Collider other)
    {
        if (other.CompareTag("fase1") && SceneManager.GetActiveScene().name == "Cenario - Fase 2")
        {
            playerDialogue.dialogues[23].TriggerDialogue();

            Destroy(other.gameObject);
        }

        if (other.CompareTag("fase2"))
        {
            playerDialogue.dialogues[8].TriggerDialogue();

            Destroy(other.gameObject);
        }

        if (other.CompareTag("fase3") && SceneManager.GetActiveScene().name == "J2 - Cenário")
        {
            playerDialogue.dialogues[10].TriggerDialogue();

            Destroy(other.gameObject);
        }

        if (other.CompareTag("fase3Ponte") && SceneManager.GetActiveScene().name == "J2 - Cenário")
        {
            playerDialogue.dialogues[19].TriggerDialogue();

            Destroy(other.gameObject);
        }

        if (other.CompareTag("fase4"))
        {
            playerDialogue.dialogues[20].TriggerDialogue();

            Destroy(other.gameObject);
        }

        if (other.CompareTag("fase5"))
        {
            playerDialogue.dialogues[21].TriggerDialogue();

            Destroy(other.gameObject);
        }

        if (other.CompareTag("fase5Fim") && SceneManager.GetActiveScene().name == "Cenario - Fase 2")
        {
            playerDialogue.dialogues[22].TriggerDialogue();
        }
    }

    private void Collectibles(Collider other)
    {
        if (other.CompareTag("collectible"))
        {
            StartCoroutine(Catching());

            switch (other.name)
            {
                case "Collectible1":
                    playerDialogue.dialogues[13].TriggerDialogue();
                    Destroy(other.gameObject);
                    break;
                case "Collectible2":
                    playerDialogue.dialogues[14].TriggerDialogue();
                    Destroy(other.gameObject);
                    break;
                case "Collectible3":
                    playerDialogue.dialogues[15].TriggerDialogue();
                    Destroy(other.gameObject);
                    break;
                default:
                    break;
            }
        }

        if (other.CompareTag("semaforo") && InventoryManager.Instance.currentItems < InventoryManager.Instance.totalItems)
        {
            playerDialogue.dialogues[16].TriggerDialogue();
        }
    }

    private void Tutorials(Collider other)
    {
        if (firstEncountersDialogues == 0)
        {
            if (other.gameObject.name == "Gerador1")
            {
                playerDialogue.dialogues[5].TriggerDialogue();

                firstEncountersDialogues++;
            }
        }

        if (other.CompareTag("ambuTutorial"))
        {
            playerDialogue.dialogues[7].TriggerDialogue();

            Destroy(other.gameObject);
        }

        if (firstEncountersDialogues == 1)
        {
            if (other.CompareTag("elevator"))
            {

                playerDialogue.dialogues[9].TriggerDialogue();
                firstEncountersDialogues++;
            }
        }
    }

    public void ToggleNavMesh()
    {
        agent.enabled = !agent.enabled;
    }

    public float GetAxisReserve(string axis)
    {
        if (axis == "Vertical" && InputManager.GetKey("Forward"))
            return 1f;
        else if (axis == "Vertical" && InputManager.GetKey("Backward"))
            return -1f;
        else if (axis == "Horizontal" && InputManager.GetKey("Right"))
            return 1f;
        else if (axis == "Horizontal" && InputManager.GetKey("Left"))
            return -1f;
        else
            return 0f;
    }
}
