using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FirstSquareController : MonoBehaviour
{
    //Obje sahnede gorunur oldugunda calisan 
    private void OnEnable()
    {
        TestEventManager.OnClicked += ChangeColor;
    }

    private void OnDisable()
    {
        TestEventManager.OnClicked -= ChangeColor;
    }


    void ChangeColor()
    {
        Color color = new Color(Random.value, Random.value, Random.value);
        GetComponent<SpriteRenderer>().color = color;
    }
}
