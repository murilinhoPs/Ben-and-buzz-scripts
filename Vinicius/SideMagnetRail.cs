using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMagnetRail : MonoBehaviour
{

    public GameObject magnet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MagnetGuide")
        {
            if (!magnet.GetComponent<SideMagnet>().invert)
            {
                if (other.name == "Top")
                {
                    magnet.GetComponent<SideMagnet>().up = true;
                }
                if (other.name == "Bot")
                {
                    magnet.GetComponent<SideMagnet>().down = true;
                }
                if (other.name == "Left")
                {
                    magnet.GetComponent<SideMagnet>().left = true;
                }
                if (other.name == "Right")
                {
                    magnet.GetComponent<SideMagnet>().right = true;
                }
            }
            if (magnet.GetComponent<SideMagnet>().invert)
            {
                if (other.name == "Top")
                {
                    magnet.GetComponent<SideMagnet>().up = true;
                }
                if (other.name == "Bot")
                {
                    magnet.GetComponent<SideMagnet>().down = true;
                }
                if (other.name == "Left")
                {
                    magnet.GetComponent<SideMagnet>().left = true;
                }
                if (other.name == "Right")
                {
                    magnet.GetComponent<SideMagnet>().right = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MagnetGuide")
        {
            if (!magnet.GetComponent<SideMagnet>().invert)
            {
                if (other.name == "Top")
                {
                    magnet.GetComponent<SideMagnet>().up = false;
                }
                if (other.name == "Bot")
                {
                    magnet.GetComponent<SideMagnet>().down = false;
                }
                if (other.name == "Left")
                {
                    magnet.GetComponent<SideMagnet>().left = false;
                }
                if (other.name == "Right")
                {
                    magnet.GetComponent<SideMagnet>().right = false;
                }
            }
            if (magnet.GetComponent<SideMagnet>().invert)
            {
                if (other.name == "Top")
                {
                    magnet.GetComponent<SideMagnet>().up = false;
                }
                if (other.name == "Bot")
                {
                    magnet.GetComponent<SideMagnet>().down = false;
                }
                if (other.name == "Left")
                {
                    magnet.GetComponent<SideMagnet>().left = false;
                }
                if (other.name == "Right")
                {
                    magnet.GetComponent<SideMagnet>().right = false;
                }
            }
        }
    }
}
