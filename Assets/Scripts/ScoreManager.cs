using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int satirlar;
    public int level = 1;

    public int numberRowsInLevel = 5; //seviyedeki satir sayisi

    private int minSatir = 1;
    private int maxSatir = 4;

    public TextMeshProUGUI rowText; //satir text
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;

    public bool isLevelUp = false;
    
    
    private void Start()
    {
        ResetFnc();
    }

    public void ResetFnc()
    {
        level = 1;
        satirlar = numberRowsInLevel * level;
        UpdateText();
    }

    //Satir Skoru
    public void RowScore(int n)
    {
        isLevelUp = false;
        n = Mathf.Clamp(n, minSatir, maxSatir); //10 satir silinse bile 1 ile 4 arasinda sinirlanacak

        switch (n)
        {
            case 1 :
                score += 30 * level;
                break;
            
            case 2 :
                score += 50 * level;
                break;
            case 3 :
                score += 150 * level;
                break;
            case 4 :
                score += 500 * level;
                break;
        }

        satirlar -= n;

        if (satirlar <= 0)
        {
            LevelUp();
        }
        
        
        UpdateText();
    }

    void UpdateText()
    {
        if (scoreText)
        {
            scoreText.text = AddZeroToScoreText(score,5);
        }

        if (levelText)
        {
            levelText.text = level.ToString();
        }

        if (rowText)
        {
            rowText.text = satirlar.ToString();
        }
    }

    string AddZeroToScoreText(int score, int rakamSayisi)
    {
        string skorStr = score.ToString();

        while (skorStr.Length < rakamSayisi)
        {
            skorStr = "0" + skorStr;
        }

        return skorStr;
    }

    public void LevelUp()
    {
        level++;
        satirlar = numberRowsInLevel * level;
        isLevelUp = true;

    }
}
