using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    [SerializeField]
    private bool isTurn=true;

    public Sprite shapeSekil; //spawnermanagerdan cagrilabilecek sekilleri eklemek icin

     GameObject[] placedEffects;

     

     private void Start()
     {
         placedEffects = GameObject.FindGameObjectsWithTag("PlacedEffect");
     }

     public void PlayPlacedEffect()
     {
         int counter = 0;
         
         //tetrisin alt objeleri icinde gerceklesitirilecek
         foreach (Transform child in gameObject.transform)
         {
             if (placedEffects[counter])
             {
                 placedEffects[counter].transform.position = new Vector3(child.position.x,child.position.y,0f);
                 //her efektin icindeki partical manager bul
                 ParticleManager particleManager = placedEffects[counter].GetComponent<ParticleManager>();

                 if (particleManager)
                 {
                     //zaten varsa calistir
                     particleManager.PlayEffect();
                 }
             }

             counter++;
         }
     }

     public void LeftMove()
    {
        transform.Translate(Vector3.left,Space.World);
    }

    public void RightMove()
    {
        transform.Translate(Vector3.right,Space.World);
    }

    public void DownMove()
    {
        transform.Translate(Vector3.down,Space.World);
    }

    public void UpMove()
    {
        transform.Translate(Vector3.up,Space.World);
    }

    public void TurnRight()
    {
        if (isTurn)
        {
            transform.Rotate(0,0,-90,Space.World);
        }
    }
    
    public void TurnLeft()
    {
        if (isTurn)
        {
            transform.Rotate(0,0,90,Space.World);
        }
    }

    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            DownMove();
            yield return new WaitForSeconds(.25f);
        }
    }

    //Saat yonunde donsun mu fonk
    public void RightRotateBtn(bool isRight)
    {
        if (isRight)
        {
            TurnRight();
        }
        else
        {
            TurnLeft();
        }
    }
    
    
}
