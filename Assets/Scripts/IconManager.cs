using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    public Sprite onIcon;
    public Sprite offIcon;

    private Image iconImg;

    public bool isCurrentIcon = true;

    private void Start()
    {
        iconImg = GetComponent<Image>();

        iconImg.sprite = (isCurrentIcon) ? onIcon : offIcon;
    }

    //icon ac kapa fonk
    public void IconTurn(bool icon)
    {
        if (!iconImg || !onIcon || !offIcon)
        {
            return;
        }

        iconImg.sprite = (icon) ? onIcon : offIcon;
    }
}
