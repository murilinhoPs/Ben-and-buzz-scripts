using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Semaforo : MonoBehaviour
{
    public GameObject finalCut;
    private bool acabou = false;

    void Update()
    {
        if (InventoryManager.Instance.currentItems >= InventoryManager.Instance.totalItems &&
        GetComponent<PlayerTriggerDialogue>().dialogues[0].dialogueManager.finished && acabou)
        {
            finalCut.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Human"))
        {
            if (InventoryManager.Instance.currentItems >= InventoryManager.Instance.totalItems)
            {
                GetComponent<PlayerTriggerDialogue>().dialogues[0].TriggerDialogue();

                acabou = true;
            }
        }
    }
}
