using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickUpManager : MonoBehaviour
{
    private bool holdingItem = false;

    [Tooltip("Qual imagem mostrar quando o player pega o item. Imagem do item")]
    public GameObject itemHud;

    [Tooltip("Define o botão de interação na UI")]
    public GameObject interactButton;
    public Text interactText;

    private GameObject grade;

    private static bool firstPiece = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("engrenagem"))
        {
            interactButton.SetActive(true);
            interactText.text = "Pegar";
        }
        if (other.CompareTag("engrenagem") && firstPiece)
        {
            interactButton.SetActive(true);
            GetComponent<PlayerTriggerDialogue>().dialogues[18].TriggerDialogue();
        }

        if (holdingItem && other.CompareTag("accessPonte"))
        {
            interactButton.SetActive(true);

            interactText.text = "Colocar";
        }

        if (holdingItem && other.CompareTag("access"))
        {
            grade = other.transform.GetChild(0).gameObject;

            interactButton.SetActive(true);

            interactText.text = "Colocar";

        }
        else if (!holdingItem && other.CompareTag("access") && firstPiece)
        {
            grade = other.transform.GetChild(0).gameObject;

            GetComponent<PlayerTriggerDialogue>().dialogues[17].TriggerDialogue();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (holdingItem && other.CompareTag("access"))
        {
            interactButton.SetActive(true);

            if (InputManager.GetKeyDown("Interact"))
            {

                grade.SetActive(false);

                interactButton.SetActive(false);

                other.GetComponent<Collider>().enabled = false;

                holdingItem = false;
                itemHud.SetActive(false);
                GetComponentInChildren<Animator>().Play("Working");
            }
        }

        if (other.CompareTag("access"))
        {
            if (Input.GetKeyDown(KeyCode.L) && Cheat.Instance.cheatActivated) // HAck
            {
                grade.SetActive(false);

                interactButton.SetActive(false);

                other.GetComponent<Collider>().enabled = false;

                holdingItem = false;
                itemHud.SetActive(false);
                GetComponentInChildren<Animator>().Play("Working");
            }
        }

        if (holdingItem && other.CompareTag("accessPonte"))
        {
            if (InputManager.GetKeyDown("Interact"))
            {
                FindObjectOfType<Drawbridge>().foreverDown = true;
                GetComponentInChildren<Animator>().Play("Working");
            }

        }

        if (other.CompareTag("accessPonte"))
        {
            if (Input.GetKeyDown(KeyCode.L) && Cheat.Instance.cheatActivated)
            {
                FindObjectOfType<Drawbridge>().foreverDown = true;
                GetComponentInChildren<Animator>().Play("Working");
            }

        }

        if (other.CompareTag("engrenagem"))
        {
            interactButton.SetActive(true);

            if (InputManager.GetKeyDown("Interact"))
            {
                other.gameObject.SetActive(false);
                itemHud.SetActive(true);
                holdingItem = true;

                interactButton.SetActive(false);

                firstPiece = false;
            }
        }
    }

    private void OnTriggerExit(Collider other) => interactButton.SetActive(false);

}
