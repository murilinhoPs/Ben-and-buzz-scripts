using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinigameFixedGears : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public MinigameGearSpin minigameMasterControl;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (minigameMasterControl.activePiston)
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
}
