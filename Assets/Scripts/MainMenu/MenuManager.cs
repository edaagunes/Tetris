using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Transform mainMenu, settingsMenu;

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private Slider musicBackSlider, fxSlider;

    private void Start()
    {
        //Bir music slider ayari yoksa en yuksek slider degerinde muzik acik olsun
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume",1);
            musicBackSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        
        //Hafizada bir ses ayari varsa onu al
        else
        {
            musicBackSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }

        fxSlider.value = 1;
    }

    //Settings 1200x de iken -1200 ile 0 konumuna cektik
    public void OpenSettingsMenu()
    {
        mainMenu.GetComponent<RectTransform>().DOLocalMoveX(-1200, .5f);
        settingsMenu.GetComponent<RectTransform>().DOLocalMoveX(0f, .5f);
    }

    public void CloseSettingsMenu()
    {
        mainMenu.GetComponent<RectTransform>().DOLocalMoveX(0, .5f);
        settingsMenu.GetComponent<RectTransform>().DOLocalMoveX(1200f, .5f);
    }

    //Play tusuna basildiginda oyun sahnesine gecilsin
    public void GamePlayScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    //Anamenuden ses ayarlarini degistirme
    public void ChangeBackGroundVolume()
    {
        musicSource.volume = musicBackSlider.value;
        //Oyun sahnesinde de bu ses ayarini kullanmak icin kaydettik
        PlayerPrefs.SetFloat("musicVolume",musicBackSlider.value);
    }
    
    //FX Sound Ayari
    public void ChangeFXVolume()
    {
        PlayerPrefs.SetFloat("FxVolume",fxSlider.value);
    }
}