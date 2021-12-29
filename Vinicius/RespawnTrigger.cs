using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{

    public int area;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Human" || other.tag == "Robot")
        {
            RespawnManager.respawnArea = area;
        }
    }

}
