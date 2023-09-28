using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using DG.Tweening;

public class SpawnerManager : MonoBehaviour
{
     [SerializeField]
     ShapeManager[] allShapes;

     [SerializeField] private Image[] shapeImages = new Image[2];
     private ShapeManager[] nextShapes = new ShapeManager[2];

     
     public ShapeManager CreateShape()
     {
          ShapeManager shape = null;
          shape = TakeNextShape();
          shape.gameObject.SetActive(true);
          shape.transform.position = transform.position;
          
          if (shape != null)
          {
               return shape;
          }
          else
          {
               return null;
          }
     }


     //Siradaki sekli once null yapacak
     public void AllShapeNull()
     {
          
          for (int i = 0; i < nextShapes.Length; i++)
          {
               nextShapes[i] = null;
          }
          FillNext();
          
     }
     //bos olan sekli doldur
     void FillNext()
     {
          for (int i = 0; i < nextShapes.Length; i++)
          {
               if (!nextShapes[i])
               {
                    nextShapes[i] = Instantiate(RandomCreateShape(),transform.position,Quaternion.identity) as ShapeManager;
                    nextShapes[i].gameObject.SetActive(false);
                    shapeImages[i].sprite = nextShapes[i].shapeSekil;
               }
          }

          StartCoroutine(ShapeImageRoutine());
     }

     IEnumerator ShapeImageRoutine()
     {
          for (int i = 0; i < shapeImages.Length; i++)
          {
               shapeImages[i].GetComponent<CanvasGroup>().alpha=0f;
               shapeImages[i].GetComponent<RectTransform>().localScale=Vector3.zero;
          }

          yield return new WaitForSeconds(.1f);

          int counter = 0;
          
          while (counter < shapeImages.Length)
          {
               shapeImages[counter].GetComponent<CanvasGroup>().DOFade(1, .6f);
               shapeImages[counter].GetComponent<RectTransform>().DOScale(1, .6f).SetEase(Ease.OutBack);
               
               counter++;

               yield return new WaitForSeconds(.4f);
          }
          
     }
     
     ShapeManager RandomCreateShape()
     {
          int randomShape = Random.Range(0, allShapes.Length);

          if (allShapes[randomShape])
          {
               return allShapes[randomShape];
          }
          else
          {
               return null;
          }
     }

     //siradaki sekli al
     ShapeManager TakeNextShape()
     {
          ShapeManager nextShape = null;

          if (nextShapes[0])
          {
               nextShape = nextShapes[0];
          }

          for (int i = 1; i < nextShapes.Length; i++)
          {
               //gelecek 2.sekli bir ust kutucuga tasi
               nextShapes[i - 1] = nextShapes[i];
               shapeImages[i - 1].sprite = nextShapes[i - 1].shapeSekil;
          }
          
          //alttaki elemani null yap son eleman yani
          nextShapes[nextShapes.Length - 1] = null;
          FillNext();
          return nextShape;
     }

     //degistirilebilecek sekil olustur
     public ShapeManager CreateHolderShape()
     {
          ShapeManager holderShape = null;
          holderShape = Instantiate(RandomCreateShape(),transform.position,Quaternion.identity) as ShapeManager;
          holderShape.transform.position = transform.position;

          return holderShape;
     }

}
