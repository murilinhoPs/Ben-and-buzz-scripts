using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Magnet : MonoBehaviour
{

    public AudioClip magnetMove;
    public AudioClip startingMove;
    public AudioClip colSound;

    public GameObject mCamera;
    public bool control;
    public bool active = false;
    public float speed = 1.5f;
    [Tooltip("compensação do tamanho do imã para o robô não entrar no imã")]
    public float difference;

    [Tooltip("Velocidade do ímã")]

    public float velocity = 0.3f;

    AudioSource aSource;

    private RaycastHit hit;
    public LayerMask rayLayer;


    [Tooltip("Apenas para referência se pegou o robô ou não. Não mexer")]
    [SerializeField] private Transform robotObject;
    [Tooltip("Apenas para referência se pegou a cabeça ou não. Não mexer")]

    [SerializeField] private Transform headObject;

    [SerializeField] private int puxado = 0;

    void Start() => aSource = gameObject.GetComponent<AudioSource>();


    void Update()
    {
        GetComponent<AudioSource>().volume = OptionsManager.effectVolume;
        if (control)
        {
            mCamera.SetActive(true);

            // Ativar ou desativar o ímã
            if (InputManager.GetKeyDown("Activate"))
            {
                active = !active;

            }

            // Testando raycast
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1000f, rayLayer))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);

                Debug.Log("Hit: " + hit.collider.name);
                // Debug.Log(" HitPoint(Vector3): " + hit.point);
            }

            MoveChild();

            #region Sounds
            if (InputManager.GetKeyDown("Left") || InputManager.GetKeyDown("Right") || InputManager.GetKeyDown("Backward") || InputManager.GetKeyDown("Forward"))
            {
                aSource.PlayOneShot(startingMove);
            }

            if (InputManager.GetKey("Left") || InputManager.GetKey("Right") || InputManager.GetKey("Forward") || InputManager.GetKey("Backward"))
            {
                aSource.clip = magnetMove;
                if (aSource.isPlaying == false) aSource.Play();
            }
            else
            {
                aSource.Stop();
            }
            #endregion

        }
        else if (!control && !DialogueManager.inDialogue)
        {
            mCamera.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (control)
        {
            // Mover o Íma
            if (InputManager.GetKey("MagnetUp"))
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.fixedDeltaTime);

            if (InputManager.GetKey("MagnetDown"))
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.fixedDeltaTime);

            if (InputManager.GetKey("MagnetLeft"))
                transform.position = new Vector3(transform.position.x - speed * Time.fixedDeltaTime, transform.position.y, transform.position.z);
            
            if (InputManager.GetKey("MagnetRight"))
                transform.position = new Vector3(transform.position.x + speed * Time.fixedDeltaTime, transform.position.y, transform.position.z); 
        }
    }


    private void MoveChild()
    {

        if (active)
        {
            puxado = 1;

            if (robotObject != null && headObject == null)
            {
                robotObject.GetChild(0).SendMessage("DisableNavMesh");

                SwapPlayerCharacter.Instance.canChange = false;

                robotObject.transform.parent = transform;

                if (robotObject.transform.position.y < transform.position.y - difference)
                {
                    robotObject.transform.position = new Vector3(robotObject.transform.position.x, robotObject.transform.position.y + 0.3f,
                    robotObject.transform.position.z);
                }
            }

            if (headObject != null && robotObject == null)
            {
                headObject.GetComponent<NavMeshAgent>().enabled = false;

                headObject.transform.parent = transform;

                if (headObject.transform.position.y < transform.position.y - 4.0f)
                {
                    headObject.transform.position = new Vector3(headObject.transform.position.x, headObject.transform.position.y + 0.3f,
                    headObject.transform.position.z);
                }
            }
        }
        else if (!active && puxado != 0)
        {
            SwapPlayerCharacter.Instance.canChange = true;


            if (robotObject != null && headObject == null)
            {
                robotObject.transform.parent = null;

                robotObject.transform.localPosition = new Vector3(hit.point.x, hit.point.y - 0.15f, hit.point.z); ;


                if (Vector3.Distance(robotObject.transform.localPosition, hit.point) <= 2)
                {
                    print("Chegou no chão");

                    robotObject.GetChild(0).SendMessage("EnableNavMesh");
                }

                puxado = 0;

                robotObject = null;
            }

            if (headObject != null && robotObject == null)
            {
                headObject.transform.parent = null;

                headObject.transform.position = new Vector3(hit.point.x, hit.point.y - 0.15f, hit.point.z);

                if (Vector3.Distance(headObject.transform.position, hit.point) <= 2)
                {
                    print("Chegou no chão");

                    headObject.GetComponent<NavMeshAgent>().enabled = true;
                }

                puxado = 0;

                headObject = null;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Cabeca")
        {
            headObject = other.transform;
        }

        if (other.gameObject.tag == "Robot")
        {
            robotObject = other.transform.parent.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Robot" || other.gameObject.tag == "Cabeca"))
        {
            robotObject = null;
            headObject = null;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Robot" || other.gameObject.tag == "Cabeca")
        {
            aSource.PlayOneShot(colSound);
        }
    }
}