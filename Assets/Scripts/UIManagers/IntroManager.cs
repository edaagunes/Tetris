using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IntroManager : MonoBehaviour
{
    public GameObject[] numbers;

    public GameObject numbersTransform;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        StartCoroutine(OpenNumbersRoutine());
    }

    IEnumerator OpenNumbersRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        numbersTransform.GetComponent<RectTransform>().DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutBack);
        numbersTransform.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);

        yield return new WaitForSeconds(0.2f);
        int counter = 0;

        while (counter<numbers.Length)
        {
            //sayiyi yukari cek
            numbers[counter].GetComponent<RectTransform>().DOLocalMoveY(0, 0.5f);
            numbers[counter].GetComponent<CanvasGroup>().DOFade(1f, 0.5f);

            //sayiyi buyutup kucult
            numbers[counter].GetComponent<RectTransform>().DOScale(2f, 0.3f).SetEase(Ease.OutBounce).OnComplete(()=>
                numbers[counter].GetComponent<RectTransform>().DOScale(1f,0.3f).SetEase(Ease.InBack));

            yield return new WaitForSeconds(1.5f);
            
            counter++;
            
            //gecen sayinin y degerini yukari cek sahnede gorunmemesi icin
            numbers[counter-1].GetComponent<RectTransform>().DOLocalMoveY(150f, 0.5f);
            
            yield return new WaitForSeconds(.1f);

        }
        
        //OnComplete bittikten sonra su fonk calistir demek
        numbersTransform.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() =>
            {
                //geri sayma bittiginde intro panelini kapat
                numbersTransform.transform.parent.gameObject.SetActive(false);
                
                //oyunu baslat
                gameManager.StartGame();
            }
            
            
            );
    }
}
