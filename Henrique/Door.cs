using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    //Este código (apesar do nome) só está utilizável para GRADES, por favor não utilize ele para portas por enquanto.
    [Tooltip("O(s) botão(ões) que precisam estar acionados para a porta abrir.")]
    public Button[] buttons;
    [Tooltip("A bool para simbolizar se a porta está fechada ou não.")]
    public bool isClosed;
    NavMeshObstacle obs;
    Animator anim;

    private void Start()
    {
        obs = GetComponent<NavMeshObstacle>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        ButtonCheck();
    }

    void ButtonCheck()
    {
        isClosed = false;

        //Checa se todos os botões estão pressionados. Se algum não estiver a porta não é aberta.
        foreach (Button b in buttons)
        {
            if (!b.activated)
            {
                isClosed = true;
            }

        }

        if (isClosed)
        {
            obs.enabled = true;
            anim.SetBool("gradeDown", false);
        }
        else
        {
            obs.enabled = false;
            anim.SetBool("gradeDown", true);
        }
    }
}
