using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private void Start() {
        Cursor.lockState = CursorLockMode.None;

    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("sair");
    }
}
