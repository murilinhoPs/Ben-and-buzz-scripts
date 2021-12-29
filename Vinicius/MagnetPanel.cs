using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPanel : MonoBehaviour
{

    public GameObject human;
    public GameObject magnet;
    public AudioClip controlSound;

    AudioSource aSource;
    public GameObject interact;

    bool inside = false;
    public bool broken;

    private bool canAnimate = false;

    // Start is called before the first frame update
    void Start()
    {
        human = GameObject.Find("Ben");
        aSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SwapPlayerCharacter.Instance.isHuman)
        {
            if (inside && !broken)
            {
                interact.SetActive(true);

                if (InputManager.GetKeyDown("Interact") && SwapPlayerCharacter.Instance.isHuman == true)
                {
                    canAnimate = !canAnimate;

                    SwapPlayerCharacter.Instance.magnetControl = !SwapPlayerCharacter.Instance.magnetControl;

                    // human.GetComponent<Human>().enabled = !human.GetComponent<Human>().enabled;
                    human.GetComponentInChildren<AnimationManager>().SetBool("magnet", canAnimate);

                    DistanceManager.Instance.canMovePlayer = !DistanceManager.Instance.canMovePlayer;

                    if (magnet.name == "Magnet")
                        magnet.GetComponent<Magnet>().control = !magnet.GetComponent<Magnet>().control;
                    else if (magnet.name == "SideMagnet")
                        magnet.GetComponent<SideMagnet>().control = !magnet.GetComponent<SideMagnet>().control;

                    aSource.PlayOneShot(controlSound);
                }
            }
            else if (inside && broken)
            {
                GetComponent<PlayerTriggerDialogue>().dialogues[0].TriggerDialogue();

            }
        }
        else interact.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Human")
        {
            inside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Human")
        {
            inside = false;
            interact.SetActive(false);
        }
    }

}
