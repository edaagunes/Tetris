using System;
using UnityEngine;
using TMPro;
public class TouchManager : MonoBehaviour
{
    public delegate void TouchEventDelegate(Vector2 swipePos);

    public static event TouchEventDelegate DragEvent;
    public static event TouchEventDelegate SwipeEvent;
    public static event TouchEventDelegate TapEvent;

    private Vector2 touchMove;

    [Range(50, 250)] public int minDragUzaklik = 100;
    
    [Range(20,250)]
    public int minSwipeDistance = 50; //minimum surukleme uzakligi 
    
    
    public bool isTxtUsed=false;

    private float tiklamaMaxSure = 0f;
    public float ekranaTiklamaSuresi = .1f;

   
    //Tap tiklandi fonk
    void Clicked()
    {
        if (TapEvent!= null)
        {
            TapEvent(touchMove);
        }
    }
    
    
    void SwipeFnc()
    {
        if (DragEvent != null)
        {
            DragEvent(touchMove);
        }
    }

    void SwipeEndFnc()
    {
        if (SwipeEvent != null)
        {
            SwipeEvent(touchMove);
        }
    }
    
   
    string SwipeTani(Vector2 surukleHareketi)
    {
        string direction = "";

        //Surukleme hareketinin x i y den buyukse ve x 0 dan buyukse sag yazdir degilse sol yazdir
        if (Mathf.Abs(surukleHareketi.x) > Mathf.Abs(surukleHareketi.y))
        {
            direction = (surukleHareketi.x >= 0) ? "sag" : "sol";
        }
        else
        {
            direction = (surukleHareketi.y >= 0) ? "yukari" : "asagi";
        }

        return direction;
    }
    private void Update()
    {
        if (Input.touchCount>0)
        {
            Touch touch = Input.touches[0];

            //Dokunmaya basladiginda
            if (touch.phase==TouchPhase.Began)
            {
                //ilk dokunusu sifira esitle
                touchMove = Vector2.zero;
                tiklamaMaxSure = Time.time + ekranaTiklamaSuresi;
                
               
            }
            
            //dokunmayi tasiyorsa 
            else if (touch.phase==TouchPhase.Moved || touch.phase==TouchPhase.Stationary)
            {
                touchMove += touch.deltaPosition;

                //parmagi tasima uzaklıgi min uzakliktan fazlaysa surukle fonk calissin
                if (touchMove.magnitude>minDragUzaklik)
                {
                    SwipeFnc();
                   
                }
            }
            else if (touch.phase==TouchPhase.Ended)
            {
                if (touchMove.magnitude>minSwipeDistance)
                {
                    SwipeEndFnc();
                }
                else if (Time.time<tiklamaMaxSure)
                {
                    Clicked();
                }
                
            }
            
            
        }
    }
}
