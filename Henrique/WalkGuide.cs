using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkGuide : MonoBehaviour
{
    //O objeto associado a esse código serve como guia para a movimentação do robô
    //Assim quando ele andar a esquerda, por exemplo, ele andará a esquerda em relação a camera.
    //O objeto também sempre segue o robô para maior precisão e evitar bugs.
    Vector3 zerothRotation;
    Quaternion qRotation;
    public Transform cam;

    private void Start()
    {
        cam = GameObject.Find("MainCamera").transform;
    }

    private void Update()
    {
        transform.LookAt(cam.position);
        zerothRotation = new Vector3(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        qRotation.eulerAngles = zerothRotation;
        transform.rotation = qRotation;
    }
}