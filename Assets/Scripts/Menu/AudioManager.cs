using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSound, effectSound;
    public AudioSource musicSource, effectSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        PlayMusic("BackgroundMusic");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSound, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void PlayEffect(string name)
    {
        Sound s = Array.Find(effectSound, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        effectSource.clip = s.clip;
        effectSource.Play();
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume / 100;
    }
    public void EffectVolume(float volume)
    {
        effectSource.volume = volume / 100;
    }
}
