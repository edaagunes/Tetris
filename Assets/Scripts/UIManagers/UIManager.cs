using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public bool isGameStop = false;
    public GameObject pausePanel;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        if (pausePanel)
        
            pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausePanelActive();
        }
    }

    //pause panel ac-kapa
    public void PausePanelActive()
    {
        if (_gameManager.isGameOver)
            return;

        isGameStop = !isGameStop;

        if (pausePanel)
        {
            pausePanel.SetActive(isGameStop);

            if (SoundManager.instance)
            {
                SoundManager.instance.SoundEffectRun(0);
                Time.timeScale = (isGameStop) ? 0 : 1; //oyun durduysa zamani durdur durmadiysa zaman aksin
            }
            
        }
    }

    
    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
