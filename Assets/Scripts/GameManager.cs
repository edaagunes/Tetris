using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{ 
    SpawnerManager spawner;
    BoardManager board;

    private ShapeManager activeShape;

    
    [FormerlySerializedAs("spawnTimer")]
    [Header("Counters")]
    [Range(0.02f,1f)] //spawnTimer icin scrollbar
    [SerializeField]
    private float downTimer=.1f; //asagi inme suresi
    private float downCounter; //asagi inme sayac
    [Range(0.02f,1f)]
    [SerializeField] private float keyPressTimer=.25f; //sag sol tuslarina basma suresi
    private float keyPressCounter;
    [Range(0.02f,1f)]
    [SerializeField] private float rotationTimer=.25f; //sag sol donme suresi
    private float rotationCounter;
    [Range(0.02f,1f)]
    [SerializeField] private float getdownTimer=.25f; //asagi tusa basma suresi
    private float getdownCounter;

    private bool isGameOver = false;

    private void Start()
    {
      //  spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerManager>();
      board = GameObject.FindObjectOfType<BoardManager>();
      spawner = GameObject.FindObjectOfType<SpawnerManager>();

      if (spawner)
      {
          if (activeShape==null)
          {
              activeShape = spawner.CreateShape();
              activeShape.transform.position = VectorToInt(activeShape.transform.position);
          }
      }
      
    }
    
    private void Update()
    {
       
        if (!board || !spawner || !activeShape || isGameOver)
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
                activeShape.LeftMove();
            }
        }
        
        //sola hareket
        else if ((Input.GetKey("left") && Time.time > keyPressCounter) || Input.GetKeyDown("left"))
        {
                activeShape.LeftMove();
                keyPressCounter = Time.time + keyPressTimer;


                if (!board.InCurrentPosition(activeShape))
                {
                    activeShape.RightMove();
                }
        }
        
        //rotation
        else if ((Input.GetKeyDown("up") && Time.time > rotationCounter))
        {
            activeShape.TurnRight();
            rotationCounter = Time.time + rotationTimer;


            if (!board.InCurrentPosition(activeShape))
            {
                activeShape.LeftMove();
            }
        }
        
        //asagi tusuyla hizli inme
        else if ((Input.GetKey("down") && Time.time > getdownTimer) || Time.time > downCounter)
        {
                downCounter = Time.time + downTimer;
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

        if (spawner)
        {
            activeShape = spawner.CreateShape();
        }
        
        board.CleanAllLine();
    }


    //Aktif shape in transform degerini tam sayiya yuvarladik,keskisnlestirdik
    Vector2 VectorToInt(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x),Mathf.Round(vector.y));
    }
}
