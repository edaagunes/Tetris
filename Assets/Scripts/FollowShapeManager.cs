using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShapeManager : MonoBehaviour
{
   private ShapeManager followShape = null;
   private bool isGroundTouch = false;

   public Color color = new Color(1f,1f,1f,0.2f);

   public void CreateFollowShape(ShapeManager realShape,BoardManager board)
   {
      //nesne yoksa olustur
      if (!followShape)
      {
         followShape = Instantiate(realShape,realShape.transform.position,realShape.transform.rotation) as ShapeManager;

         followShape.name = "TakipShape";

         SpriteRenderer[] allSprite = followShape.GetComponentsInChildren<SpriteRenderer>();

         foreach (SpriteRenderer sr in allSprite)
         {
            sr.color = color;
         }
      }
      
      //nesne varsa hareket etsin
      else
      {
         followShape.transform.position = realShape.transform.position;
         followShape.transform.rotation = realShape.transform.rotation;
      }

      isGroundTouch = false;

      while (!isGroundTouch)
      {
         followShape.DownMove();
         if (!board.InCurrentPosition(followShape))
         {
            followShape.UpMove();
            isGroundTouch = true;
         }
      }
   }

   public void ResetFnc()
   {
      Destroy(followShape.gameObject);
   }
}
