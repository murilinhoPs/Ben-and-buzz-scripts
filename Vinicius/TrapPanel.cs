using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPanel : MonoBehaviour
{

    public MagnetTrap Magnet;
    private bool inside;
    public GameObject interact;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inside)
        {
            interact.SetActive(true);
            if (InputManager.GetKeyDown("Interact"))
            {
                Magnet.Deactivate();
                interact.SetActive(false);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Human")
        {
            inside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Human")
        {
            inside = false;
            interact.SetActive(false);
        }
    }
}
