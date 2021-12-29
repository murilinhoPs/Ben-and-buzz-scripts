using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    //Os botões possuem um simples sistema de triggers.
    public bool activated;
    Animator anim;
    public AudioClip effect;

    private void Start()
    {
        activated = false;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        GetComponent<AudioSource>().volume = OptionsManager.effectVolume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot") || other.CompareTag("Human") || other.CompareTag("Cabeca"))
        {
            anim.Play("StepOn");
            GetComponent<AudioSource>().PlayOneShot(effect);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Robot") || other.CompareTag("Cabeca"))
            activated = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Robot") || other.CompareTag("Human"))
        {
            activated = false;
            anim.Play("StepOff");
            GetComponent<AudioSource>().PlayOneShot(effect);
        }
    }
}
