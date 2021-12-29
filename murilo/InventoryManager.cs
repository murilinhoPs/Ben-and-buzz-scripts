using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public int totalItems = 3;

    public int currentItems = 0;

    public GameObject key;

    public Text keyText;

    public static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<InventoryManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("DistanceManager");
                    _instance = container.AddComponent<InventoryManager>();
                }
            }

            return _instance;
        }
    }

    private void Update()
    {
        if (currentItems > 0)
        {
            key.SetActive(true);

            keyText.text = "X " + currentItems.ToString();
        }

        if (Input.GetKeyDown(KeyCode.K)&& Cheat.Instance.cheatActivated)
            currentItems = 3;
    }
}
