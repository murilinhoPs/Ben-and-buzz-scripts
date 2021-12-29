using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool playOnStart = false;
    public DialogueObject dialogue;

    [HideInInspector] public DialogueManager dialogueManager;

    private int status = 0; // Saber se ele pode pular ou nao

    void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Start()
    {
        if (playOnStart)
            TriggerDialogue();
    }

    void Update()
    {
        if (InputManager.GetKeyDown("Dialogue"))
        {
            if (dialogueManager.typying)
            {
                if (status == 0)
                {
                    dialogueManager.EndTypyingSentence();

                    status = 1;
                }
            }
            else
            {
                dialogueManager.NextSentence();

                status = 0;
            }
        }
    }

    public void TriggerDialogue()
    {
        dialogueManager.InitDialogue(dialogue);

        // Quando o jogador chegar em certas partes do jogo, ele da InitDialogue no DialogueTrigger
        // referenciado no editor (public DialogueTrigger)
    }
}
