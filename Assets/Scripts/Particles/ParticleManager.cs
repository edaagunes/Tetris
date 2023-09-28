using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem[] allEffects;

    private void Start()
    {
        //cocuklarinda da particle ariyor
        allEffects = GetComponentsInChildren<ParticleSystem>();
    }

    public void PlayEffect()
    {
        foreach (ParticleSystem effect in allEffects)
        {
            //effectlerin ust uste binmemesi icin durdur ve oynat
            effect.Stop();
            effect.Play();
        }
    }
}
