using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum InteractStates { NONE, BOX, HANDLE, CARRIER, ELEVATOR, MAGNET, JUMP, PLATFORM, AMBU }

public class Robot : MonoBehaviour
{
    public InteractStates currentInteractState;

    #region Variables
    Human humano;
    RobotJump robotJump;
    public Transform guide;
    public Transform carrierPos;

    [Tooltip("Velocidade de movimentação do robô")]
    public float speed = 10.0f;

    [Tooltip("Esse valor auxilia para a rotação do robô ser mais suave")]
    public float turnSmoothing = 150f;
    public NavMeshAgent agent;
    private Vector3 walkDirection;

    public bool carryingWheelchair;
    public GameObject interactInput, interactFix, fixParticle;
    public bool canCarryWheelchair;

    private bool isNotTired = true;
    private bool resetStamina = true;

    private StaminaManager staminaManager;
    [HideInInspector] public PlayerTriggerDialogue playerDialogue;

    public bool fixing;
    private bool canPlay = true;

    public GameObject firstTimeAmbu;
    public Transform ambuPos;

    AnimationManager humanoAnimationManager;

    public AnimationManager robotAnimation; // robotAnimation.SetBool("walkinNormal", true);

    public float dashSpeed = 10;

    private bool dashingWithBen;
    public ParticleSystem brokenSmoke;

    bool playingCarry;
    #endregion

    private void Start()
    {
        carryingWheelchair = SwapPlayerCharacter.Instance.carrying;

        staminaManager = GetComponent<StaminaManager>();
        playerDialogue = GetComponent<PlayerTriggerDialogue>();
        robotJump = GetComponent<RobotJump>();
        agent = transform.parent.GetComponent<NavMeshAgent>();

        humano = GameObject.Find("Ben").GetComponent<Human>();
        humanoAnimationManager = humano.GetComponentInChildren<AnimationManager>();

        // guide = GameObject.Find("RobotWalkGuide").transform;
    }

    void Update()
    {
        if (SwapPlayerCharacter.Instance.isHuman)
            robotAnimation.SetBool("walkingNormal", false);

        if (DistanceManager.Instance.canMoveRobot)
        {
            verifyStamina();

            if (Cheat.Instance.cheatActivated && !SwapPlayerCharacter.Instance.isHuman)
            {
                if (Input.GetKeyDown(KeyCode.O)&& Cheat.Instance.cheatActivated)
                {
                    staminaManager.restoreStamina(10f, true);
                }
            }

            if (isNotTired)
            {
                if (!SwapPlayerCharacter.Instance.isHuman)
                {
                    WheelchairSetup();

                    // Walk();
                    robotJump.VerifyJump(agent, robotAnimation);

                    if (GetComponent<Caixote>().isCarried)
                    {
                        if (!playingCarry)
                        {
                            robotAnimation.GetComponent<Animator>().Play("PegandoCaixa");

                            playingCarry = true;
                        }
                        staminaManager.takeAndResetStamina(isNotTired, 0.2f, resetStamina);
                    }
                    else
                    {
                        robotAnimation.SetBool("soltandoCaixa", true);
                        playingCarry = false;
                    }
                }

                if (staminaManager.currentStamina < staminaManager.totalStamina)
                {
                    if (DistanceManager.Instance.canFix && SwapPlayerCharacter.Instance.isHuman)
                    {
                        interactFix.SetActive(true);

                        if (InputManager.GetKeyDown("Repair/Heal"))
                        {
                            fixing = true;
                            interactFix.SetActive(false);

                            fixParticle.SetActive(true);
                            StartCoroutine(humano.Fixing());

                            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
                            staminaManager.takeAndResetStamina(false, 0f, resetStamina);

                            playerDialogue.dialogues[3].TriggerDialogue();
                        }
                    }
                    else
                    {
                        fixing = false;
                        interactFix.SetActive(false);
                    }
                }

                if (brokenSmoke.isEmitting) brokenSmoke.Stop();
            }
            else
            {
                if (carryingWheelchair)
                {
                    // Parar de carregar o player
                    carryingWheelchair = false;

                    humano.transform.parent.GetComponent<NavMeshAgent>().enabled = true;
                    humano.transform.parent.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                    humano.transform.parent.transform.parent = null;
                }

                if (canPlay && staminaManager.startFull)
                {
                    playerDialogue.dialogues[1].TriggerDialogue();
                    canPlay = false;
                }

                if (DistanceManager.Instance.canFix && !GetComponent<Caixote>().isCarried && SwapPlayerCharacter.Instance.isHuman)
                {
                    interactFix.SetActive(true);

                    if (!staminaManager.startFull) // parte da oficina
                    {
                        if (InputManager.GetKeyDown("Repair/Heal"))
                        {
                            fixing = true;

                            firstTimeAmbu.SetActive(true);
                            interactFix.SetActive(false);

                            fixParticle.SetActive(true);
                            StartCoroutine(humano.Fixing());

                            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
                            staminaManager.takeAndResetStamina(isNotTired, 0f, resetStamina);

                            playerDialogue.dialogues[0].TriggerDialogue();

                            SwapPlayerCharacter.Instance.canChange = true;
                            canPlay = true;
                        }

                    }
                    else if (InputManager.GetKeyDown("Repair/Heal"))
                    {
                        fixing = true;
                        interactFix.SetActive(false);

                        fixParticle.SetActive(true);
                        StartCoroutine(humano.Fixing());

                        transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
                        staminaManager.takeAndResetStamina(isNotTired, 0f, resetStamina);
                        playerDialogue.dialogues[2].TriggerDialogue();

                        canPlay = true;
                    }
                }
                else
                    interactFix.SetActive(false);

                if (!brokenSmoke.isEmitting) brokenSmoke.Play();
            }
        }
        else
        {
            robotAnimation.SetBool("walkingNormal", false);
        }

        SwapPlayerCharacter.Instance.carrying = carryingWheelchair;
    }

    void FixedUpdate()
    {
        if (DistanceManager.Instance.canMoveRobot && isNotTired && !SwapPlayerCharacter.Instance.isHuman)
            Walk();
    }

    IEnumerator PerformDash(Vector3 direction)
    {
        dashingWithBen = true;

        agent.speed = 24f;
        agent.acceleration = 10f;

        agent.Move(direction * dashSpeed);

        yield return new WaitForSeconds(0.5f);

        yield return new WaitUntil(() => agent.transform.position.magnitude >= agent.nextPosition.magnitude);

        agent.isStopped = true;

        agent.speed = 4f;
        agent.acceleration = 8f;

        dashingWithBen = false;

    }

    void Walk()
    {
        if (humano.GetAxisReserve("Horizontal") != 0f || humano.GetAxisReserve("Vertical") != 0f)
        {

            var horizontalInput = humano.GetAxisReserve("Horizontal") * Time.fixedDeltaTime;
            var verticalInput = humano.GetAxisReserve("Vertical") * Time.fixedDeltaTime;

            var direction = new Vector3(horizontalInput, 0f, verticalInput);
            walkDirection = guide.TransformDirection(direction * (-1f)) * speed;

            if (!dashingWithBen)
                agent.Move(walkDirection);

            Quaternion targetRotation = Quaternion.LookRotation(walkDirection, Vector3.up);
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothing * Time.fixedDeltaTime);

            transform.rotation = newRotation;

            robotAnimation.SetBool("jumping", false);

            if (GetComponent<Caixote>().isCarried && playingCarry)
            {
                robotAnimation.SetBool("pegandoCaixa", false);
                robotAnimation.SetBool("walkingCaixa", true);
                robotAnimation.SetBool("walkingNormal", false);
            }

            if (carryingWheelchair)
            {
                if (InputManager.GetKeyDown("Dash"))
                {
                    StartCoroutine(PerformDash(walkDirection));
                }

                staminaManager.takeAndResetStamina(isNotTired, 0.5f, true);
                humanoAnimationManager.SetBool("empurrado", true);
                robotAnimation.SetBool("idleEmpurra", false);
                robotAnimation.SetBool("walkingNormal", false);
                robotAnimation.SetBool("walkEmpurra", true);
            }
            else
            {
                humanoAnimationManager.SetBool("empurrado", false);
                robotAnimation.SetBool("walkEmpurra", false);
                robotAnimation.SetBool("idleEmpurra", false);
                robotAnimation.SetBool("walkingNormal", true);
            }
        }
        else
        {
            //agent.isStopped = true;
            humanoAnimationManager.SetBool("empurrado", false);

            if (GetComponent<Caixote>().isCarried)
            {
                robotAnimation.SetBool("walkingCaixa", false);
                robotAnimation.SetBool("walkingNormal", false);
            }

            if (carryingWheelchair)
            {
                robotAnimation.SetBool("walkEmpurra", false);
                robotAnimation.SetBool("walkingNormal", false);
                robotAnimation.SetBool("idleEmpurra", true);
            }
            else
            {
                robotAnimation.SetBool("walkEmpurra", false);
                robotAnimation.SetBool("idleEmpurra", false);
                robotAnimation.SetBool("walkingNormal", false);
            }
        }


    }

    public void WheelchairSetup()
    {
        //Quando o robô estiver na posição certa e a tecla Shift estiver pressionada ele carregará o inventor.
        //Esse código ajusta a posição de ambos para q a movimentação adequada aconteça.
        //O inventor se torna child do robô para garantir q ambos se movam facilmente.
        if (canCarryWheelchair)
        {
            if (InputManager.GetKeyDown("Carry") && !carryingWheelchair)
            {
                humano.transform.parent.GetComponent<NavMeshAgent>().enabled = false;
                agent.enabled = false;

                SwapPlayerCharacter.Instance.canChange = false;

                var newRotation = new Quaternion();
                newRotation.eulerAngles = new Vector3(0f, 0f, 0f);

                humano.transform.rotation = newRotation;
                carrierPos.transform.rotation = newRotation;
                transform.rotation = newRotation;

                // agent.SetDestination(carrierPos.transform.position);

                transform.parent.position = carrierPos.transform.position;

                // transform.parent.LookAt(carrierPos.transform);

                humano.transform.parent.transform.parent = transform;
                humano.transform.parent.localPosition = new Vector3(0f, -0.5f, 0.5f);

                agent.enabled = true;

                carryingWheelchair = true;
            }
        }

        if (InputManager.GetKeyUp("Carry"))
        {
            humano.transform.parent.GetComponent<NavMeshAgent>().enabled = true;

            SwapPlayerCharacter.Instance.canChange = true;

            humano.transform.parent.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            humano.transform.parent.transform.parent = null;
            carryingWheelchair = false;
        }
    }

    void verifyStamina()
    {
        if (staminaManager.currentStamina <= 0)
            isNotTired = false;
        else
            isNotTired = true;
    }

    public IEnumerator Ambu()
    {
        DistanceManager.Instance.canMovePlayer = false;
        DistanceManager.Instance.canMoveRobot = false;

        transform.rotation = Quaternion.LookRotation(-ambuPos.forward, Vector3.up);
        transform.parent.position = ambuPos.position;

        robotAnimation.SetBool("ambu", true);

        yield return new WaitForSeconds(3.0f);

        robotAnimation.SetBool("ambu", false);
        DistanceManager.Instance.canMovePlayer = true;
        DistanceManager.Instance.canMoveRobot = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CarrierPosition") && !SwapPlayerCharacter.Instance.isHuman)
        {
            humano.interactPlayer.SetActive(false);
            canCarryWheelchair = true;
            interactInput.SetActive(true);

            currentInteractState = InteractStates.CARRIER;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CarrierPosition"))
        {
            canCarryWheelchair = false;
            interactInput.SetActive(false);

            currentInteractState = InteractStates.NONE;
        }
    }

    public void ToggleNavMesh()
    {
        agent.enabled = !agent.enabled;
    }

    public void DisableNavMesh()
    {
        agent.enabled = false;
    }

    public void EnableNavMesh()
    {
        agent.enabled = true;
    }

    public void EmpurraParado()
    {
        robotAnimation.SetBool("idleEmpurra", true);
        robotAnimation.SetBool("walkingNormal", false);
        robotAnimation.SetBool("walkEmpurra", false);
    }

    public void EmpurraAndando()
    {
        robotAnimation.SetBool("idleEmpurra", false);
        robotAnimation.SetBool("walkingNormal", false);
        robotAnimation.SetBool("walkEmpurra", true);
    }
}

#region Obsolete

#endregion
