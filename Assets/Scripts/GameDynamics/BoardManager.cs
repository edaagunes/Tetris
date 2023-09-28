using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private Transform tilePrefab;

    public int height = 22;
    public int width = 10;

    private Transform[,] grid;

    public int completedRaw = 0;

    public ParticleManager[] rawEffects = new ParticleManager[4];

    private void Awake()
    {
        grid = new Transform[width, height];
    }

    private void Start()
    {
        CreateEmptyBoxes();
    }

    bool isInBoard(int x,int y)
    {
        return (x >= 0 && x < width && y >= 0); //sekil board icerisinde mi kontrol sarti
    }

    bool isSquareFull(int x, int y,ShapeManager shape)
    {
        return (grid[x, y] != null && grid[x, y].parent != shape.transform); //kendi shape i ile degil baska shapelerin transformarliyla karsilastirsin
        
        
    }

    public bool InCurrentPosition(ShapeManager shape)
    {
        //eger butun kareler board icindeyse
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = VectorToInt(child.position);

            if (!isInBoard((int)pos.x,(int)pos.y))
            {
                return false; //Eger karelerden biri board disina ciktiysa false
            }

            if (pos.y < height)
            {
                if (isSquareFull((int)pos.x,(int)pos.y,shape))
                {
                    return false;
                }
            }

        }

        return true;
    }
    
    void CreateEmptyBoxes()
    {
        if (tilePrefab != null)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {//(0,0)-->(1,0)---(10,0) after (0,0)-->(1,1)---(10,22)
                    Transform tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    tile.name = "x " + x.ToString() + " ," + "y" + y.ToString();
                    tile.parent = this.transform; //board nesnesi icinde olussun
                }
            }
        }
        else
        {
            print("tile prefab null reference!");
        }
    }

    public void InGrid(ShapeManager shape)
    {
        if (shape==null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = VectorToInt(child.position);
            grid[(int)pos.x, (int)pos.y] = child;
        }
    }

    bool isLineFull(int y)
    {
        //dongu icerisinde tum satirlari dolasacak ve eger satırda bosluk varsa false donecek
        for (int x = 0; x < width; ++x)
        {
            if (grid[x,y] == null)
            {
                return false;
            }
        }

        return true;
    }

    void CleanLine(int y)
    {
        //satiri temizle
        for (int x = 0; x < width; ++x)
        {
            if (grid[x,y] != null)
            {
                Destroy(grid[x,y].gameObject);
            }

            grid[x, y] = null;
        }
    }

    void DownOneLine(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x,y] != null)
            {
                //bir alt satirdaki elemanlari ust satirdakilere esitledik
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x,y-1].position += Vector3.down;
            }
        }
    }

    void DownAllLine(int startY)
    {
        for (int i = startY; i < height; ++i)
        {
            DownOneLine(i);
        }
    }

    public IEnumerator CleanAllLine()
    {
        completedRaw = 0;

        //once efekt calissin
        for (int y = 0; y < height; ++y)
        {
            if (isLineFull(y))
            {
                RunRawEffect(completedRaw,y);
                completedRaw++;
            }
        }

        yield return new WaitForSeconds(.5f);
        
        //sonra bloklar yok olsun
        for (int y = 0; y < height; y++)
        {
            if (isLineFull(y))
            {
                CleanLine(y);
                DownAllLine(y+1);

                yield return new WaitForSeconds(.2f);
                y--;
            }
        }
    }

    //GameOver icin shape board disina tasti mi
    public bool isSpillOut(ShapeManager shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= height - 1)
            {
                return true;
            }
        }

        return false;
    }
    
    Vector2 VectorToInt(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x),Mathf.Round(vector.y));
    }

    void RunRawEffect(int kacinciSatir, int y)
    {
       // if (rawEffect)
       //{
            //effecti silinen y satir degerine cektik
       //     rawEffect.transform.position = new Vector3(0, y, 0);
       //     rawEffect.PlayEffect();
       // }

       if (rawEffects[kacinciSatir])
       {
           rawEffects[kacinciSatir].transform.position = new Vector3(0, y, 0);
           rawEffects[kacinciSatir].PlayEffect();
       }
    }
    


}
