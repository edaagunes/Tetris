using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{ 
    SpawnerManager spawner;
    BoardManager board;
    private ScoreManager scoreManager;

    private ShapeManager activeShape;

    
    [FormerlySerializedAs("spawnTimer")]
    [Header("Counters")]
    [Range(0.02f,1f)] //spawnTimer icin scrollbar
    [SerializeField]
    private float downTimer=.5f; //asagi inme suresi
    private float downCounter; //asagi inme sayac

    private float downLevelCounter; //asagi inme level sayac
    
    [Range(0.02f,1f)]
    [SerializeField] private float keyPressTimer=.25f; //sag sol tuslarina basma suresi
    private float keyPressCounter;
    [Range(0.02f,1f)]
    [SerializeField] private float rotationTimer=.25f; //sag sol donme suresi
    private float rotationCounter;
    [Range(0.02f,1f)]
    [SerializeField] private float getdownTimer=.25f; //asagi tusa basma suresi
    private float getdownCounter;

    public bool isGameOver = false;
    
    public bool isRight = true; //saat yonu mu
    public IconManager rotateIcon;

    public GameObject gameOverPanel;
    private void Start()
    {
      //  spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerManager>();
      board = GameObject.FindObjectOfType<BoardManager>();
      spawner = GameObject.FindObjectOfType<SpawnerManager>();
      scoreManager = GameObject.FindObjectOfType<ScoreManager>();

      if (spawner)
      {
          if (activeShape==null)
          {
              activeShape = spawner.CreateShape();
              activeShape.transform.position = VectorToInt(activeShape.transform.position);
          }
      }

      if (gameOverPanel)
      {
          gameOverPanel.SetActive(false);
      }

      downLevelCounter = downTimer;
    }
    
    private void Update()
    {
       
        if (!board || !spawner || !activeShape || isGameOver || !scoreManager)
        {
            return;
        }

        InputController();
    }
    

    private void InputController()
    {
        //saga hareket
        if ((Input.GetKey("right") && Time.time > keyPressCounter) || Input.GetKeyDown("right"))
        {
            activeShape.RightMove();
            keyPressCounter = Time.time + keyPressTimer;

            if (!board.InCurrentPosition(activeShape))
            {
                SoundManager.instance.SoundEffectRun(1);
                activeShape.LeftMove();
            }
            else
            {
                SoundManager.instance.SoundEffectRun(3);
            }
        }
        
        //sola hareket
        else if ((Input.GetKey("left") && Time.time > keyPressCounter) || Input.GetKeyDown("left"))
        {
                activeShape.LeftMove();
                keyPressCounter = Time.time + keyPressTimer;


                if (!board.InCurrentPosition(activeShape))
                {
                    SoundManager.instance.SoundEffectRun(1);
                    activeShape.RightMove();
                }
                else
                {
                    SoundManager.instance.SoundEffectRun(3);
                }
        }
        
        //rotation
        else if ((Input.GetKeyDown("up") && Time.time > rotationCounter))
        {
            activeShape.TurnRight();
            rotationCounter = Time.time + rotationTimer;


            if (!board.InCurrentPosition(activeShape))
            {
                SoundManager.instance.SoundEffectRun(1);
                activeShape.LeftMove();
            }
            else
            {
                isRight = !isRight;
                if (rotateIcon)
                {
                    rotateIcon.IconTurn(isRight);
                }
                
                SoundManager.instance.SoundEffectRun(3);
            }
        }
        
        //asagi tusuyla hizli inme
        else if (((Input.GetKey("down") && Time.time > getdownTimer)) || Time.time > downCounter)
        {
                downCounter = Time.time + downLevelCounter;
                getdownCounter = Time.time + getdownTimer;

                if (activeShape)
                {
                    activeShape.DownMove();

                    if (!board.InCurrentPosition(activeShape))
                    {
                        
                        if (board.isSpillOut(activeShape))
                        {
                            activeShape.UpMove();
                            
                            isGameOver = true;

                            if (gameOverPanel)
                            {
                                gameOverPanel.SetActive(true);
                                SoundManager.instance.SoundEffectRun(6);
                            }
                           
                            
                            SoundManager.instance.SoundEffectRun(6);
                        }
                        else
                        {
                            ShapePlaced();
                        } 
                        
                    }
                }
        }

    }

    private void ShapePlaced()
    {
        keyPressCounter = Time.time;
        getdownCounter = Time.time;
        rotationCounter = Time.time;
        
        
        
        activeShape.UpMove();

        board.InGrid(activeShape);
        SoundManager.instance.SoundEffectRun(5);

        if (spawner)
        {
            activeShape = spawner.CreateShape();
        }
        
        board.CleanAllLine();

        if (board.completedRaw > 0)
        {
            scoreManager.RowScore(board.completedRaw);

            if (scoreManager.isLevelUp)
            {
                SoundManager.instance.SoundEffectRun(7);
                
                //asagi inme suresini hizlandirma
                downLevelCounter = downTimer - Mathf.Clamp(((float)scoreManager.level - 1) * .1f, 0.05f, 1f);
            }
            else
            {
                if (board.completedRaw > 1)
                {
                    SoundManager.instance.LocalSoundRun();
                }
            }
            
          

            SoundManager.instance.SoundEffectRun(4);
        }
    }


    //Aktif shape in transform degerini tam sayiya yuvarladik,keskinlestirdik
    Vector2 VectorToInt(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x),Mathf.Round(vector.y));
    }

    //Icon donme yonu
    public void RotationIconDirection()
    {
        isRight = !isRight;
        activeShape.RightRotateBtn(isRight);

        //gecerli sekil board disinda ise
        if (!board.InCurrentPosition(activeShape))
        {
            activeShape.RightRotateBtn(!isRight);
            SoundManager.instance.SoundEffectRun(3);
        }
        else
        {
            if (rotateIcon)
            {
                rotateIcon.IconTurn(isRight);
            }
            SoundManager.instance.SoundEffectRun(2);
        }
    }
}
