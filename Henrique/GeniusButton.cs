using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeniusButton : MonoBehaviour
{
    public MinigameGenius mg;
    public Sprite mainGearSprite;
    bool spinning;
    float time = 0f;

    private void Update()
    {
        if (spinning)
        {
            GetComponent<RectTransform>().Rotate(new Vector3(0f, 0f, (360f / 2f) * Time.deltaTime));
            time += Time.deltaTime;
            if (time >= 2f)
            {
                time = 0f;
                spinning = false;
            }
        }
    }

    public void InsertSequence()
    {
        mg.insertionIcons[mg.currentInsertion].sprite = mainGearSprite;
        mg.currentInsertion++;
        if (mg.currentInsertion >= mg.sequenceLength)
        {
            mg.currentPhase = MinigameGenius.Phases.CHECKUP;
            mg.StartCoroutine(mg.SequenceCheckup());
        }

        time = 0f;
        spinning = true;
    }
}
