using UnityEngine;
/*
//TouchManager.cs Delegate yapisi

//Delegate Yapisi
//Delegate Tanimlamasi

 public delegate void MathDelegate(int x, int y);

    
//Delegate objesi olusturma
public MathDelegate fZincir;

private void Start()
{
    //Fonksiyonlari zincirleme sekilde cagirma
    fZincir = Sum;
    fZincir += Multiplication;

    fZincir(4, 5);

    //Carpma fonk devre disi birakma
    fZincir -= Multiplication;

    fZincir(5, 6);

    //Lambda expression
    fZincir += (int a, int b) =>
    {
        int result = a - b;
        print(result);
    };
}

//Delgate icerisindeki parametrelerle ayni olmalı(int/int)
void Sum(int a, int b)
{
    int result = a + b;
    print(result);
}

void Multiplication(int a, int b)
{
    int result = a * b;
    print(result);
}

*/

//Event ve Delegate Farklari;
//Event objesi tanimlarken event kelimesi kullanilir
//Event objelerine fonk atamasi yaparken += seklinde tanimlanir(= degil)
//Eventleri cagirmayi sadece event objesinin tanimlandigi siniflarda yapilir

public class TestEventManager : MonoBehaviour
{
    //Delegate Tanimlamasi
    public delegate void ClickAction();
    
    //Event Tanimlamasi(baska yerlerden de erismek icin satatic)
    public static event ClickAction OnClicked;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // if (OnClicked!=null)
            // {
            //     OnClicked();
            // }
            
            OnClicked?.Invoke();
        }
    }
}
