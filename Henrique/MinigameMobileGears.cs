using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MinigameMobileGears : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    bool clicked;
    Vector3 distDifference;

    bool foundPin;
    GameObject selectedPin;
    [HideInInspector] public bool pinned;

    [HideInInspector] public MinigameGearSpin minigameMasterControl;

    GearOuterPart outerPart;

    private void Start()
    {
        outerPart = GetComponentInChildren<GearOuterPart>();
    }

    void Update()
    {
        if (!pinned || !minigameMasterControl.path.Contains(gameObject))
        {
            outerPart.spinning = false;
            outerPart.spinDirection = 0;
            Quaternion target = Quaternion.Euler(0, 0, 0);
            transform.rotation = target;
        }

        if (clicked)
        {
            Vector3 pos = Input.mousePosition + distDifference;

            transform.position = new Vector3(Mathf.Clamp(pos.x, -5f, 2036.75f),
              Mathf.Clamp(pos.y, -5f, 785f));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (minigameMasterControl.activePiston)
            return;

        Color ccc = Color.white;
        ccc.a = 0.5f;
        GetComponent<Image>().color = ccc;
        distDifference = transform.position - Input.mousePosition;
        clicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (minigameMasterControl.activePiston)
            return;

        Color ccc = Color.white;
        ccc.a = 1f;
        GetComponent<Image>().color = ccc;
        clicked = false;

        if (foundPin)
        {
            transform.position = selectedPin.transform.position;
            pinned = true;
        }
        else
            pinned = false;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (minigameMasterControl.activePiston && pinned)
        {
            if (minigameMasterControl.activePiston.currentState == MinigamePiston.CurrentState.FIRST_GEAR)
            {
                minigameMasterControl.activePiston.gear1 = GetComponentInChildren<GearOuterPart>();
                minigameMasterControl.activePiston.currentState = MinigamePiston.CurrentState.SECOND_GEAR;
            }

            if (minigameMasterControl.activePiston.currentState == MinigamePiston.CurrentState.SECOND_GEAR)
            {
                if (GetComponentInChildren<GearOuterPart>() == minigameMasterControl.activePiston.gear1)
                    return;

                minigameMasterControl.activePiston.gear2 = GetComponentInChildren<GearOuterPart>();
                minigameMasterControl.activePiston.Positioning();
                minigameMasterControl.activePiston.currentState = MinigamePiston.CurrentState.WORKING;
                minigameMasterControl.activePiston = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GearPins"))
        {
            foundPin = true;
            selectedPin = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GearPins"))
        {
            foundPin = false;
            selectedPin = null;
        }
    }
}
