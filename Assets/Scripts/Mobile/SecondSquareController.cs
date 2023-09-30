using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondSquareController : MonoBehaviour
{
    private void OnEnable()
    {
        TestEventManager.OnClicked += TurnObject;
    }

    private void OnDisable()
    {
        TestEventManager.OnClicked -= TurnObject;
    }


    void TurnObject()
    {
        transform.Rotate(new Vector3(0,0,30));
    }
}
