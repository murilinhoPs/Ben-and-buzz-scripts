using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MinigamePathButton : MonoBehaviour, IPointerClickHandler
{
    public enum ButtonType { NULL, SIMPLE, CHARGE, UNMOVABLE, WHITE, ENTRY, EXIT }
    public Sprite[] buttonTextures;
    public bool charged;

    [HideInInspector] public bool nextToEntry;
    [HideInInspector] public bool nextToCharge;

    public ButtonType startingButton;
    [HideInInspector] public ButtonType currentButton;

    public MinigamePathButton[] adjacencies;
    [HideInInspector] public MinigamePathButton whiteAdjacent;

    Image image;

    public MinigamePath minigamePath;

    private void Start()
    {
        currentButton = startingButton;
        image = GetComponent<Image>();
    }

    private void Update()
    {
        ColorCheck();
        AdjacencyCheck();
    }

    void ColorCheck()
    {
        switch (currentButton)
        {
            case ButtonType.NULL:
                image.color = Color.clear;
                break;
            case ButtonType.SIMPLE:
                image.sprite = buttonTextures[0];
                break;
            case ButtonType.CHARGE:
                image.sprite = buttonTextures[1];
                break;
            case ButtonType.UNMOVABLE:
                image.sprite = buttonTextures[2];
                break;
            case ButtonType.WHITE:
                image.sprite = buttonTextures[3];
                break;
            case ButtonType.ENTRY:
                image.sprite = buttonTextures[4];
                charged = true;
                break;
            case ButtonType.EXIT:
                image.sprite = buttonTextures[4];
                break;
            default:
                break;
        }
    }

    void AdjacencyCheck()
    {
        if ((currentButton == ButtonType.CHARGE && !nextToEntry && !nextToCharge) || !(currentButton == ButtonType.CHARGE))
            if (minigamePath.path.Contains(this)) minigamePath.path.Remove(this);

        if (currentButton == ButtonType.NULL || currentButton == ButtonType.UNMOVABLE || currentButton == ButtonType.WHITE)
        {
            whiteAdjacent = null;
            charged = false;
            nextToEntry = false;
            nextToCharge = false;
        }

        if (currentButton == ButtonType.NULL || currentButton == ButtonType.UNMOVABLE || currentButton == ButtonType.WHITE
             || currentButton == ButtonType.ENTRY || currentButton == ButtonType.EXIT) return;

        whiteAdjacent = null;
        charged = false;
        nextToEntry = false;
        nextToCharge = false;

        foreach (MinigamePathButton item in adjacencies)
        {
            if (item.currentButton == ButtonType.WHITE) whiteAdjacent = item;

            if (currentButton == ButtonType.CHARGE)
            {
                if (item.currentButton == ButtonType.ENTRY)
                {
                    if (!minigamePath.path.Contains(this)) minigamePath.path.Add(this);
                    nextToEntry = true;
                }
                if (item.currentButton == ButtonType.CHARGE)
                {
                    if (!minigamePath.path.Contains(this) && item.charged) minigamePath.path.Add(this);
                    nextToCharge = true;
                }

                if (charged && item.currentButton == ButtonType.EXIT) item.charged = true;

                if (minigamePath.path.Contains(this)) charged = true;
                else charged = false;
            }
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (currentButton == ButtonType.NULL || currentButton == ButtonType.UNMOVABLE || currentButton == ButtonType.WHITE
             || currentButton == ButtonType.ENTRY || currentButton == ButtonType.EXIT) return;

        if (whiteAdjacent)
        {
            whiteAdjacent.currentButton = currentButton;
            currentButton = ButtonType.WHITE;
        }
    }
}