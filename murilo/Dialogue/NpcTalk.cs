using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcTalk : MonoBehaviour
{
    private PlayerTriggerDialogue playerDialogue;

    public GameObject interactKey;

    private bool canInteract;

    public GameObject cutFinal;

    private void Start()
    {
        playerDialogue = GetComponent<PlayerTriggerDialogue>();
    }

    private void Update()
    {
        if (canInteract)
        {
            if (InputManager.GetKeyDown("Interact"))
            {
                playerDialogue.dialogues[0].TriggerDialogue();
                interactKey.SetActive(false);
                canInteract = false;
            }
        }

        if (this.gameObject.name == "Gran")
        {
            print(playerDialogue.dialogues[0].dialogueManager.currentSentence);
            if (playerDialogue.dialogues[0].dialogueManager.currentSentence == " Ali esta ela!")
                cutFinal.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Human"))
        {
            interactKey.SetActive(true);
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Human"))
        {
            interactKey.SetActive(false);
            canInteract = false;
        }
    }
}