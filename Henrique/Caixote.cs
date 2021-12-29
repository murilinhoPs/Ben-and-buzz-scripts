using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Caixote : MonoBehaviour
{
    [Tooltip("Assegura que a tecla E, quando apertada, só gere resultado se o robô estiver perto.")]
    public bool active;
    [Tooltip("É acionada se o robô estiver carregando a caixa.")]
    public bool isCarried;

    [Tooltip("A caixa de texto que avisa que a tecla 'E' é utilizada para interagir.")]
    [SerializeField] public GameObject interactKey;

    private Transform boxTransform;

    [Tooltip("A posição que a caixa tem que ficar")]
    public Transform boxPos;

    [Tooltip("Posição para devolver a caixa no chão")]
    public Transform dropPos;

    private Robot robotScript;

    private void Start()
    {
        robotScript = GetComponent<Robot>();
    }

    private void Update()
    {
        if (SwapPlayerCharacter.Instance.isHuman)
            active = false;

        if (!active)
        {
            interactKey.SetActive(false);

            return;
        }
        else
        {
            if (InputManager.GetKeyDown("Interact"))
            {
                if (!isCarried)
                {
                    isCarried = true;

                    boxTransform.GetComponent<NavMeshAgent>().enabled = false;

                    //Faz do robô o parent da caixa, assegurando q o movimento dele tbm mova a caixa.
                    boxTransform.position = boxPos.position;
                    boxTransform.rotation = boxPos.rotation;
                    boxTransform.parent = boxPos.transform;//GameObject.Find("Robot").transform;

                    //TODO: animação dele pegando a caixa e ser o 
                    //transform da mão dele e dps costas

                    interactKey.SetActive(false);
                }
                else
                {
                    //Faz com q o robô pare de segurar a caixa e levar ela para outros lugares.
                    isCarried = false;

                    boxTransform.position = dropPos.position;

                    boxTransform.parent = null;

                    boxTransform.GetComponent<NavMeshAgent>().enabled = true;

                    boxTransform = null;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cabeca") && !SwapPlayerCharacter.Instance.isHuman)
        {
            interactKey.SetActive(true);

            robotScript.currentInteractState = InteractStates.BOX;
        }

        if (other.CompareTag("Cabeca"))
        {
            boxTransform = other.transform;

            active = true;
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cabeca")
        {
            active = false;

            boxTransform = null;

            robotScript.currentInteractState = InteractStates.NONE;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cabeca"))
        {
            boxTransform = other.transform;

            active = true;
        }
    }
}
