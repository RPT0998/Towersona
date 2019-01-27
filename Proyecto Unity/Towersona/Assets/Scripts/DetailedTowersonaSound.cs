﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailedTowersonaSound : MonoBehaviour
{
    public Camera enabledReference;

    public AudioClip eatingSfx;
    public AudioClip shitSfx;
    public AudioClip lookingAtFoodSfx;
    public AudioClip happySfx;

    private AudioSource source;

    private void Update()
    {
        source.enabled = enabledReference.enabled;
    }

    public void PlayEating()
    {
        source.clip = eatingSfx;
        source.Play();
    }

    public void PlayTakenShit()
    {
        source.clip = shitSfx;
        source.Play();
    }
    
    public void PlayLookingAtFood()
    {
        source.clip = lookingAtFoodSfx;
        source.Play();
    }

    public void PlayHappy()
    {
        source.clip = happySfx;
        source.Play();
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
}
