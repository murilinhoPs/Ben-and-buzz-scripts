using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Human") || other.CompareTag("Robot"))
        {
            InventoryManager.Instance.currentItems++;

            Destroy(gameObject);
        }
    }
}
