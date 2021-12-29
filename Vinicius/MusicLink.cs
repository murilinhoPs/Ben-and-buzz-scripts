using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLink : MonoBehaviour
{
    void Update()
    {
        GetComponent<AudioSource>().volume = OptionsManager.musicVolume;
    }
}
