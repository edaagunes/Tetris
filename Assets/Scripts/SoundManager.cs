using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class SoundManager : MonoBehaviour
{
    //SingletonClass
    public static SoundManager instance;
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioSource musicSource;

    private AudioClip randomMusicClip;

    public bool isMusicRun=true;
    public bool isEffectRun = true;

    [SerializeField] private AudioSource[] soundEffects;

    [SerializeField] private AudioSource[] vocalClips;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        randomMusicClip = RandomChoose(musicClips);
        BackgroundMusic(randomMusicClip);
    }

    AudioClip RandomChoose(AudioClip[] clips)
    {
        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        return randomClip;
    }

    public void BackgroundMusic(AudioClip musicClip)
    {
        if (!musicClip || !musicSource || !isMusicRun)
        {
            return;
        }

        musicSource.clip = musicClip;
        musicSource.Play();
    }

    void MusicUpdate()
    {
        if (musicSource.isPlaying!=isMusicRun)
        {
            if (isMusicRun)
            {
                randomMusicClip = RandomChoose(musicClips);
                BackgroundMusic(randomMusicClip);
            }
            else
            {
                musicSource.Stop();
            }
        }
    }

    public void MusicTurnOff()
    {
        isMusicRun = !isMusicRun;
        MusicUpdate();
    }

    public void SoundEffectRun(int whichSound)
    {
        if (isEffectRun && whichSound < soundEffects.Length)
        {
            soundEffects[whichSound].Stop();
            soundEffects[whichSound].Play();
        }
    }

    public void FXTurnOff()
    {
        isEffectRun = !isEffectRun;
    }

    public void LocalSoundRun()
    {
        if (isEffectRun)
        {
            AudioSource source = vocalClips[Random.Range(0, vocalClips.Length)];
            source.Stop();
            source.Play();
        }
        
    }
    
}
