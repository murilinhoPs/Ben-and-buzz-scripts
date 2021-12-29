using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PortaLevadica : MonoBehaviour
{
    [Tooltip("O(s) botão(ões) que precisam estar acionados para a porta abrir.")]
    public Button[] buttons;
    [Tooltip("A bool para simbolizar se a porta está fechada ou não.")]
    public bool isClosed;
    NavMeshObstacle obs;

    bool ponteUp = false;

    private void Start()
    {
        obs = GetComponent<NavMeshObstacle>();
    }

    private void Update()
    {
        ButtonCheck();
    }

    /*void ButtonCheck()
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
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<MeshCollider>().enabled = true;
        }
        else
        {
            obs.enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;
        }
    }*/

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

        if (!isClosed)
        {
            Debug.Log("A");
            //if (!ponteUp)
            {
                ponteUp = true;
                StartCoroutine(PonteHold());
            }
        }
        else
            ponteUp = false;
    }

    IEnumerator PonteHold()
    {
        yield return new WaitForSeconds(0);
        if (ponteUp)
        {
            Debug.Log("B");
            obs.enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;
            yield break;
        }
        else
        {
            Debug.Log("C");
            yield return new WaitForSeconds(5);
            obs.enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<MeshCollider>().enabled = true;
        }
        /*yield return new WaitForSeconds(5);
        obs.enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<MeshCollider>().enabled = true;
        ponteUp = false;*/
    }
}
