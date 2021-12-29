using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameMemory : MonoBehaviour
{
    SpriteRenderer[] leftColumn;
    SpriteRenderer[] rightColumn;

    Color[] leftColors = new Color[5];
    Color[] rightColors = new Color[5];

    Color[] colorList = new Color[] { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta };

    Color selectedColorL, selectedColorR, buttonColor;

    private void Start()
    {
        leftColumn = GameObject.Find("LeftColumn").GetComponentsInChildren<SpriteRenderer>();
        rightColumn = GameObject.Find("RightColumn").GetComponentsInChildren<SpriteRenderer>();
        buttonColor = leftColumn[0].color;

        Randomize(colorList);
        for (int i = 0; i < leftColors.Length; i++)
        {
            leftColors[i] = colorList[i];
        }

        Randomize(colorList);
        for (int i = 0; i < rightColors.Length; i++)
        {
            rightColors[i] = colorList[i];
        }
    }

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hitInfo)
        {
            Debug.Log(hitInfo.rigidbody.gameObject.name);
        }
    }

    static void Randomize<T>(T[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            int j = Random.Range(i, items.Length);
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }
}
