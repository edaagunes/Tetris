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

    public IconManager musicIcon;
    public IconManager fxIcon;
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

        //Hafizadaki ses ayarini kullan ve burayi guncelle
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume");
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
        musicIcon.IconTurn(isMusicRun);
    }

    public void SoundEffectRun(int whichSound)
    {
        if (isEffectRun && whichSound < soundEffects.Length)
        {
            soundEffects[whichSound].volume = PlayerPrefs.GetFloat("FxVolume");
            
            soundEffects[whichSound].Stop();
            soundEffects[whichSound].Play();
        }
    }

    public void FXTurnOff()
    {
        isEffectRun = !isEffectRun;
        
        fxIcon.IconTurn(isEffectRun);
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
