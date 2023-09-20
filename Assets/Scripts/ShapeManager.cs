using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    [SerializeField]
    private bool isTurn=true;

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
    
}
