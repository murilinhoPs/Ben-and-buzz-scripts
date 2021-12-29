using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> sentences;
    public static bool inDialogue;

    [SerializeField] private AudioClip buzzSound;

    [HideInInspector] public AudioSource audioSource;
    public AudioClip typyingSound;
    [HideInInspector] public bool typying;

    private string[] splitSentence; //Dividindo o Nome e o texto que vai aparecer
    [HideInInspector] public string currentSentence;

    private Coroutine end, adding;

    private AnimationManager benAnim;
    private AnimationManager buzzAnim;

    [HideInInspector] public bool finished;

    private void Awake()
    {
        sentences = new Queue<string>();

        audioSource = GetComponent<AudioSource>();

        benAnim = FindObjectOfType<Human>().animationManager;
        buzzAnim = FindObjectOfType<Robot>().robotAnimation;
    }

    private void Start()
    {
        audioSource.volume = OptionsManager.effectVolume / 2.5f;
    }

    public void InitDialogue(DialogueObject dialogue)
    {
        inDialogue = true;

        // print("Starting the: " + dialogue.dialogueName);

        sentences.Clear();

        DistanceManager.Instance.canMovePlayer = false;
        DistanceManager.Instance.canMoveRobot = false;
        SwapPlayerCharacter.Instance.canChange = false;

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        UIManager.Instance.ToggleDialogueContainer(true);

        NextSentence();
    }

    public void NextSentence()
    {
        // print("Sentences count: " + sentences.Count);

        audioSource.volume = OptionsManager.effectVolume / 3f;

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        finished = false;
        typying = true;

        splitSentence = sentences.Dequeue().Split(':');
        currentSentence = splitSentence[1];

        VerifyType();

        if (adding != null)
            StopCoroutine(adding);
        adding = StartCoroutine(TypyingSentence(currentSentence));
    }

    private void VerifyType()
    {

        var sentenceName = splitSentence[0];

        var sentence = splitSentence[1];

        if ((sentenceName.ToLower() == "ben" || sentenceName.ToLower() == "anna") && sentence.Contains("+"))
            CheckAnim(sentence, sentenceName);

        if (sentenceName.ToLower() == "cam")
        {
            var dividedSentence = sentence.Split(','); // "Cam: NomeDaCamera, 2.5" (time)

            string cameraName = dividedSentence[0].Replace(" ", "");

            float time = float.Parse(dividedSentence[1], CultureInfo.InvariantCulture.NumberFormat);

            StartCoroutine(CutscenesCameras.Instance.activeOne(cameraName, time));

            NextSentence(); // Continua para a proxima fala mas com a camera ativada

            return;
        }

        if (sentenceName.ToLower() == "dica")
        {
            UIManager.Instance.SetDialogueUi(ben: false, anna: false, hint: true,
            gran: false, npc: false);
            // print(sentenceName + " : " + currentSentence);
            return;
        }

        if (sentenceName.ToLower() == "ben")
        {
            UIManager.Instance.SetDialogueUi(ben: true, anna: false, hint: false,
            gran: false, npc: false);
            // print("ben sentece -> " + sentenceName + " : " + currentSentence);
            return;
        }

        if (sentenceName.ToLower() == "npc")
        {
            UIManager.Instance.SetDialogueUi(ben: false, anna: false, hint: false,
            gran: false, npc: true);
            return;
        }

        if (sentenceName.ToLower() == "anna")
        {
            UIManager.Instance.SetDialogueUi(ben: false, anna: true, hint: false,
            gran: false, npc: false);
            return;
        }

        if (sentenceName.ToLower() == "gran")
        {
            UIManager.Instance.SetDialogueUi(ben: false, anna: false,
            gran: true, hint: false, npc: false);
            return;
        }

        if (sentenceName.ToLower() == "buzz")
        {
            audioSource.pitch = 1;
            audioSource.volume = 9f;
            audioSource.PlayOneShot(buzzSound);

            buzzAnim.Trigger("talking");

            NextSentence();
            return;
        }

    }

    private void CheckAnim(string sentence, string sentenceName)
    {
        var dividedSentence = sentence.Split('+');

        var animSentence = dividedSentence[1].Split(',');

        string animSentenceName = animSentence[0];
        string animSentenceValue = animSentence[1];

        if (animSentenceName != "comunicadorIdle") benAnim.SetBool("comunicadorIdle", false);

        if (animSentenceValue == "trigger")
        {
            // if (animSentenceName == "falandoEnd" || animSentenceName == "comunicadorEnd")
            // {
            //     benAnim.Trigger(animSentenceName);

            //     // print("sentenceNameCheckAnim -> " + dividedSentence[0]);
            //     NextSentence();
            //     return;
            // }]

            benAnim.Trigger(animSentenceName);
        }
        else
            benAnim.SetBool(animSentenceName, Convert.ToBoolean(animSentenceValue));

    }

    public void EndTypyingSentence()
    {
        StopCoroutine(adding);

        audioSource.Stop();

        UIManager.Instance.dialogueText.text = currentSentence.Split('+')[0];

        end = StartCoroutine("finishedTipying");
    }

    private IEnumerator TypyingSentence(string sentence)
    {
        UIManager.Instance.dialogueText.text = "";

        var dividedSentence = sentence.Split('+')[0];

        foreach (char letter in dividedSentence.ToCharArray())
        {
            UIManager.Instance.dialogueText.text += letter;

            audioSource.PlayOneShot(typyingSound);

            yield return 0.1f;

            audioSource.pitch = UnityEngine.Random.Range(1.10f, 1.40f);
        }

        typying = false;
    }
    private IEnumerator finishedTipying()
    {
        yield return new WaitForSeconds(0.6f);

        typying = false;
    }

    private IEnumerator Fim()
    {
        finished = true;

        yield return new WaitForSeconds(0.5f);

        finished = false;
    }

    private void EndDialogue()
    {
        finished = true;

        UIManager.Instance.ToggleDialogueContainer(false);

        inDialogue = false;

        sentences.Clear();

        DistanceManager.Instance.canMovePlayer = true;
        DistanceManager.Instance.canMoveRobot = true;
        SwapPlayerCharacter.Instance.canChange = true;

        StartCoroutine(Fim());
    }
}
