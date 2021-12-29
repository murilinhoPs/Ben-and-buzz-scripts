using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    [Tooltip("O(s) botão(ões) que precisam estar acionados para a escada virar uma rampa.")]
    public Button[] buttons;
    [Tooltip("A bool para simbolizar se é rampa ou não.")]
    public bool isRamp;
    //public bool ingrime;
    public bool hasAnim = true;

    Animator anim;
    bool hasPlayed;
    AudioClip change;

    private void Start()
    {
        if (hasAnim)
            anim = GetComponent<Animator>();
        // transform.Find("escada:pCube16").GetComponent<MeshRenderer>().enabled = false;
    }

    private void Update()
    {
        GetComponent<AudioSource>().volume = OptionsManager.effectVolume * 0.5f;
        RampCheck();
        //AnimationTest();
    }

    void RampCheck()
    {
        if (buttons.Length != 0)
        {
            isRamp = true;

            //Checa se todos os botões estão pressionados. Se algum não estiver a rampa não é acionada.
            foreach (Button b in buttons)
            {
                if (!b.activated)
                {
                    isRamp = false;
                }
            }
        }
        if (hasAnim)
        {
            if (isRamp)
            {
                anim.SetBool("isRamp", true);
            }
            else
            {
                anim.SetBool("isRamp", false);
            }
        }
    }

    void AnimationTest()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (null != anim)
            {
                anim.Play("StairToRamp", 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (null != anim)
            {
                anim.Play("RampToStair", 0, 0);
            }
        }
    }
}
