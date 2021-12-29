using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelchaitBarrier : MonoBehaviour
{
    //Um collider simples q, se o parent do objeto não for rampa, não permite q o inventor passe.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Human"
            && transform.parent.GetComponent<Ramp>().isRamp)
        {
            Debug.Log("A");
            other.transform.Translate(0f, 0f, -1f);
        }
    }
}
