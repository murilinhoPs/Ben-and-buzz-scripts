using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text dialogueText;
    public GameObject dialogueBen, dialogueAnna, dialogueHint, dialogueContainer,
    dialogueGran, dialogueNpc;

    [HideInInspector] public bool typying;

    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void SetDialogueCutscene(string charName, string dialogue, string animToPlay = "")
    {
        ToggleDialogueContainer(true);

        dialogueText.text = dialogue;

        if (charName.ToLower() == "") ToggleDialogueContainer(false);

        if (charName.ToLower() == "gran")
        {
            SetDialogueUi(ben: false, anna: false, hint: false, gran: true, npc: false);

            return;
        }

        if (charName.ToLower() == "ben")
        {
            SetDialogueUi(ben: true, anna: false, hint: false, gran: false, npc: false);
            return;
        }

        if (charName.ToLower() == "anna")
        {
            SetDialogueUi(ben: false, anna: true, hint: false, gran: false, npc: false);
            return;
        }
    }

    public void SetDialogueUi(bool ben, bool anna, bool hint, bool npc, bool gran)
    {
        dialogueBen.SetActive(ben);
        dialogueAnna.SetActive(anna);
        dialogueGran.SetActive(gran);
        dialogueNpc.SetActive(npc);
        dialogueHint.SetActive(hint);
    }

    public void ToggleDialogueContainer(bool active)
    {
        dialogueContainer.SetActive(active);
    }

    private IEnumerator TypyingSentence(string sentence)
    {
        dialogueText.text = "";

        var dividedSentence = sentence.Split('+')[0];

        typying = true;

        foreach (char letter in dividedSentence.ToCharArray())
        {
            dialogueText.text += letter;

            dialogueManager.audioSource.PlayOneShot(dialogueManager.typyingSound);

            yield return 0.1f;

            dialogueManager.audioSource.pitch = UnityEngine.Random.Range(1.10f, 1.40f);
        }

        typying = false;
    }
}
