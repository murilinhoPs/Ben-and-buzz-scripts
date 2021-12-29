using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinigamePiston : MonoBehaviour, IPointerClickHandler
{
    public GearOuterPart gear1, gear2;
    Vector3 originalPos;
    Vector2 originalSize;
    Quaternion originalRot;

    public enum CurrentState { NULL, FIRST_GEAR, SECOND_GEAR, WORKING }
    public CurrentState currentState = CurrentState.NULL;

    [HideInInspector] public MinigameGearSpin minigameMasterControl;

    private void Awake()
    {
        originalPos = GetComponent<RectTransform>().localPosition;
        originalSize = GetComponent<RectTransform>().sizeDelta;
        originalRot = GetComponent<RectTransform>().localRotation;
    }

    private void Update()
    {
        if (currentState == CurrentState.WORKING)
        {
            if (!gear1.adjacencies.Contains(gear2.gameObject)) gear1.adjacencies.Add(gear2.gameObject);
            if (!gear2.adjacencies.Contains(gear1.gameObject)) gear2.adjacencies.Add(gear1.gameObject);

            if (gear1.parentGear && gear2.parentGear)
            {
                if (!gear1.parentGear.pinned || !gear2.parentGear.pinned)
                    ResetPiston();
            }
            else if (!gear1.parentGear && gear2.parentGear)
            {
                if (!gear2.parentGear.pinned)
                    ResetPiston();
            }
            else if (gear1.parentGear && !gear2.parentGear)
            {
                if (!gear1.parentGear.pinned)
                    ResetPiston();
            }
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Right)
            ResetPiston();

        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (currentState == CurrentState.NULL && !minigameMasterControl.activePiston)
            {
                currentState = CurrentState.FIRST_GEAR;
                minigameMasterControl.activePiston = this;
            }
        }
    }

    public void Positioning()
    {
        Vector3 pos1, pos2;
        pos1 = gear1.transform.position - new Vector3(0f, 50f, 0f);
        pos2 = gear2.transform.position - new Vector3(0f, 50f, 0f);

        float height = Vector3.Distance(pos1, pos2) + 70f;
        float width = (400f / 2500f) * height;
        GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Clamp(width, 0f, 100f), height);

        Vector3 newPos = (pos1 + pos2) / 2;
        transform.position = newPos;

        Vector3 targetDir = pos1 - pos2;
        float angle = Vector3.SignedAngle(targetDir, -transform.up, transform.forward);
        Quaternion newRot = Quaternion.Euler(0, 0, -angle);
        transform.rotation = newRot;
    }

    public void ResetPiston()
    {
        if (gear1 && gear1.adjacencies.Contains(gear2.gameObject)) gear1.adjacencies.Remove(gear2.gameObject);
        if (gear2 && gear2.adjacencies.Contains(gear1.gameObject)) gear2.adjacencies.Remove(gear1.gameObject);
        if (gear1) gear1.spinning = false;
        if (gear2) gear2.spinning = false;
        gear1 = null;
        gear2 = null;
        GetComponent<RectTransform>().localPosition = originalPos;
        GetComponent<RectTransform>().localRotation = originalRot;
        GetComponent<RectTransform>().sizeDelta = originalSize;
        currentState = CurrentState.NULL;
    }
}
