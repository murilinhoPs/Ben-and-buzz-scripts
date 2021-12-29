using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearOuterPart : MonoBehaviour
{
    public MinigameMobileGears parentGear;
    [HideInInspector] public GameObject innerCounterpart;
    [HideInInspector] public MinigameGearSpin minigameMasterControl;

    public List<GameObject> adjacencies;

    public enum GearType { START, END, MIDDLE };
    public GearType gt;
    [HideInInspector] public bool spinning;
    [HideInInspector] public int spinDirection;

    private void Start()
    {
        parentGear = transform.parent.GetComponent<MinigameMobileGears>();
        innerCounterpart = transform.parent.Find("InnerPart").gameObject;
    }

    private void Update()
    {

        if (gt == GearType.START)
        {
            minigameMasterControl.adjacentToStart = adjacencies;
            spinning = true;
            spinDirection = 1;
        }

        if (gt == GearType.END)
        {
            if (!minigameMasterControl.path.Contains(transform.parent.gameObject))
            {
                spinning = false;
                spinDirection = 0;
                Quaternion target = Quaternion.Euler(0, 0, 0);
                transform.parent.rotation = target;
            }
        }

        if (minigameMasterControl.path.Contains(transform.parent.gameObject))
        {
            foreach (GameObject item in adjacencies)
            {
                if (!minigameMasterControl.path.Contains(item.transform.parent.gameObject))
                {
                    minigameMasterControl.path.Add(item.transform.parent.gameObject);
                    item.GetComponent<GearOuterPart>().spinning = true;
                }

                if (spinDirection != 0) item.GetComponent<GearOuterPart>().spinDirection = -spinDirection;
            }
        }

        if (spinning)
        {
            transform.parent.Rotate(0, 0, spinDirection * 20 * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.name == "OuterPart")
        {
            GearType xxx = other.GetComponent<GearOuterPart>().gt;
            if ((xxx == GearType.START) || (xxx == GearType.MIDDLE && other.transform.parent.GetComponent<MinigameMobileGears>().pinned)
                || (xxx == GearType.END))
                if ((gt == GearType.START) || (gt == GearType.MIDDLE && parentGear.pinned) || (gt == GearType.END))
                    if (!adjacencies.Contains(other.gameObject)) adjacencies.Add(other.gameObject);
        }

        if (other.name == "InnerPart" && other.gameObject != innerCounterpart)
        {
            int aaa = minigameMasterControl.innerParts.IndexOf(other.gameObject);
            if (adjacencies.Contains(minigameMasterControl.outerParts[aaa]))
                adjacencies.Remove(minigameMasterControl.outerParts[aaa]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "OuterPart")
            if (adjacencies.Contains(other.gameObject)) adjacencies.Remove(other.gameObject);
    }
}
