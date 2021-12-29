using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public GameObject button;

    void Start()
    {
    }

    private void Update()
    {
        videoPlayer.SetDirectAudioVolume(0, OptionsManager.musicVolume);
        if (videoPlayer.time >= videoPlayer.length - 3)
            EnableButton();

        if (Input.GetKeyDown(KeyCode.Escape))
            EnableButton();
    }

    void EnableButton()
    {
        button.SetActive(true);
    }
}
