using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerManager : MonoBehaviour
{
     [SerializeField]
     ShapeManager[] allShapes;

    

     public ShapeManager CreateShape()
     {
          int randomShape = Random.Range(0, allShapes.Length);
          ShapeManager shape = Instantiate(allShapes[randomShape],transform.position,Quaternion.identity) as ShapeManager;
         
          if (shape)
          {
               return shape;
          }
          else
          {
               return null;
          }
     }

}
