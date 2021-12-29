using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [Tooltip("Define o total de stamina do personagem")]
    public float totalStamina = 10;

    [Tooltip("Valor atual da stamina")]
    [Range(0, 20)] public float currentStamina;

    [Tooltip("Visualisar o valor da stamina na UI através de um slider")]
    public Slider staminaSlider;

    public Slider staminaSliderBack;

    public bool startFull = true;

    void Start()
    {
        if (startFull)
        {
            currentStamina = totalStamina;
            staminaSlider.value = totalStamina;
            staminaSliderBack.value = totalStamina;
        }
        else
        {
            currentStamina = 0;
            staminaSlider.value = 0;
            staminaSliderBack.value = 0;

            takeAndResetStamina(false, 0, true);
        }
    }

    public void takeAndResetStamina(bool canMove, float takeStamina, bool canFix)
    {
        if (canMove)
        {
            startFull = true;

            currentStamina -= takeStamina * Time.fixedDeltaTime;

            staminaSlider.value -= takeStamina * Time.fixedDeltaTime;
            staminaSliderBack.value -= takeStamina * Time.fixedDeltaTime;

        }
        if (!canMove && canFix)
        {
            if (InputManager.GetKeyDown("Repair/Heal"))
                delayStamina();
        }
    }


    public void restoreStamina(float addStamina, bool fullfil)
    {
        if (!fullfil)
        {
            if (currentStamina <= totalStamina)
            {
                currentStamina += addStamina * Time.fixedDeltaTime;

                staminaSlider.value += addStamina * Time.fixedDeltaTime;
                staminaSliderBack.value += addStamina * Time.fixedDeltaTime;

            }
        }
        else
            delayStamina();
    }

    public void StaminaSound(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    private void delayStamina()
    {
        currentStamina = totalStamina;

        staminaSlider.value = currentStamina;
        staminaSliderBack.value = currentStamina;
    }
}
