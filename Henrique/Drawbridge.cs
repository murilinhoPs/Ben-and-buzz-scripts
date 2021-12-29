using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drawbridge : MonoBehaviour
{
    public bool isDown;
    public NavMeshObstacle obs;

    public Button[] buttons;
    [HideInInspector] public int buttonCount;

    AudioSource audioS;
    public AudioClip change;

    [HideInInspector] public bool foreverDown;

    Animator anim;
    string currentAnimation;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDown) currentAnimation = "FullDown";
        else currentAnimation = "FullUp";

        if (isPlaying(currentAnimation))
        {
            if (!audioS.isPlaying) audioS.PlayOneShot(change);
        }
        else
            audioS.Stop();

        GetComponent<AudioSource>().volume = OptionsManager.effectVolume;
        Check();
    }

    void Check()
    {
        buttonCount = 0;
        foreach (Button b in buttons)
        {
            if (b.activated)
                buttonCount++;
        }

        if (buttonCount < 2)
            isDown = false;
        else if (buttonCount >= 2)
            isDown = true;

        if (foreverDown) isDown = true;

        if (isDown)
        {
            anim.SetBool("ponteUp", false);
            obs.enabled = false;
        }
        else
        {
            anim.SetBool("ponteUp", true);
            obs.enabled = true;
        }
    }

    bool isPlaying(string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
}
