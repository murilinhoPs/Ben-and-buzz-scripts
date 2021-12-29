using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bridge : MonoBehaviour
{
    [Tooltip("O(s) botão(ões) que precisam estar acionados para a porta abrir.")]
    public Button button;
    public Transform bridge;
    public NavMeshObstacle obs;
    Animator anim;
    bool buttonPressed = false;
    Vector3 posIni, posFin;
    float lerpQuant = 0f;

    private void Start()
    {
        posIni = new Vector3(2.2f, 0f, 0f);
        posFin = bridge.localPosition;
        bridge.localPosition = posIni;

        obs.enabled = true;
        anim = bridge.GetComponent<Animator>();
    }

    private void Update()
    {
        if (button.activated && !buttonPressed)
            StartCoroutine(PonteAppear());

        if (buttonPressed)
        {
            if (lerpQuant <= 1f)
            {
                bridge.localPosition = Vector3.Lerp(posIni, posFin, lerpQuant);
                lerpQuant += (1f / 3f) * Time.deltaTime;
            }
        }
    }

    IEnumerator PonteAppear()
    {
        buttonPressed = true;
        yield return new WaitForSeconds(3f);
        obs.enabled = false;
        anim.Play("PonteAppear");
    }
}
