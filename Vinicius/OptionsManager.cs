using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{

    public Slider musicSlider;
    public Slider effectSlider;
    public Slider brightSlider;
    public Slider sensibilitySlider;

    private static GameObject instance;

    public static float musicVolume = 0.5f;
    public static float effectVolume = 0.5f;
    public static float brightness = 0.5f;
    public static float sensiblity = 0.55f;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
            
    }

    // Update is called once per frame
    void Update()
    {
        if (musicSlider == null)
        {
            musicSlider = GameObject.Find("SliderMusica").GetComponent<Slider>();
            musicSlider.value = musicVolume;
        }
        if(effectSlider == null)
        {
            effectSlider = GameObject.Find("SliderEfeitos").GetComponent<Slider>();
            effectSlider.value = effectVolume;
        }
        if (sensibilitySlider == null)
        {
            sensibilitySlider = GameObject.Find("SliderSensibility").GetComponent<Slider>();
            sensibilitySlider.value = sensiblity;
        }

        musicVolume = musicSlider.value;
        effectVolume = effectSlider.value;
        brightness = brightSlider.value;
        sensiblity = sensibilitySlider.value;
    }
}
