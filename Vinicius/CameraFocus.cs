using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public static string activeCamera = "Human";

    [SerializeField] private GameObject[] cams;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (activeCamera == "Human")
        {
            cams[0].SetActive(true);
            cams[1].SetActive(false);
        }
        else if (activeCamera == "Robot")
        {
            cams[1].SetActive(true);
            cams[0].SetActive(false);
        }
    }
}
