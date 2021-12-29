using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadLetter : MonoBehaviour
{
    public Scrollbar scrollbar;

    private bool movingDown;
    private bool movingUp;

    public float moveValue;

    public GameObject loading;

    private void Start()
    {
        loading.SetActive(false);
    }

    private void Update()
    {
        if (movingDown)
        {
            if (scrollbar.value >= 0)
                scrollbar.value -= moveValue * Time.deltaTime;
        }

        if (movingUp)
        {
            if (scrollbar.value <= 1)
                scrollbar.value += moveValue * Time.deltaTime;
        }
    }

    public void MoveDown(bool value)
    {
        movingDown = value;
    }

    public void MoveUp(bool value)
    {
        movingUp = value;
    }
}