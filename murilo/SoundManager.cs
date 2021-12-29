using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[System.Serializable]
public class Sound
{
    [Tooltip("Nome do seu som")]
    public string nome;
    [Tooltip("Clipe que será tocado")]
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

    [Tooltip("Se vai tocar em loop ou não")]
    public bool loop;

    [Tooltip("Se vai tocar em loop no inicio do jogo ou nao")]
    public bool onAwake;
}


public class SoundManager : MonoBehaviour
{
    public Sound[] sons;

    void Awake()
    {
        foreach (Sound som in sons)
        {
            som.source = gameObject.AddComponent<AudioSource>();
            som.source.clip = som.clip;
            som.source.volume = som.volume;
            som.source.pitch = som.pitch;
            som.source.loop = som.loop;
            som.source.playOnAwake = som.onAwake;
        }
    }

    private void Update()
    {
        foreach (Sound som in sons)
        {
            som.source.GetComponent<AudioSource>().volume = OptionsManager.effectVolume;
        }
    }

    public void Play(string nome)
    {
        Sound som = Array.Find(sons, sound => sound.nome == nome);

        if (som == null) return;

        som.source.Play();
    }

    public void Stop(string nome)
    {
        Sound som = Array.Find(sons, sound => sound.nome == nome);

        som.source.Stop();
    }
}
