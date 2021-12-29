using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour
{

    //Script para parentear o jogador no elevador para ele não cair enquanto estiver movimentando.
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Human")
        {
            collision.transform.parent = transform;
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Human")
        {
            collision.transform.parent = transform;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Human")
        {
            collision.transform.parent = null;
        }
    }

}
