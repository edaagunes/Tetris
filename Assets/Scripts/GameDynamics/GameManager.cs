using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    SpawnerManager spawner;
    BoardManager board;
    FollowShapeManager followShape;

    private ScoreManager scoreManager;

    private ShapeManager activeShape;
    private ShapeManager holderShape;

    public Image holderShapeImg;

    private bool isHolderShapeChange = true;

    private bool isMove = true; //hareket etsin mi

    public ParticleManager[] levelUpEffect = new ParticleManager[5];

    public ParticleManager[] gameOverEffect = new ParticleManager[5];

    [FormerlySerializedAs("spawnTimer")]
    [Header("Counters")]
    [Range(0.02f, 1f)] //spawnTimer icin scrollbar
    [SerializeField]
    private float downTimer = .5f; //asagi inme suresi

    private float downCounter; //asagi inme sayac

    private float downLevelCounter; //asagi inme level sayac

    [Range(0.02f, 1f)] [SerializeField] private float keyPressTimer = .25f; //sag sol tuslarina basma suresi
    private float keyPressCounter;
    [Range(0.02f, 1f)] [SerializeField] private float rotationTimer = .25f; //sag sol donme suresi
    private float rotationCounter;
    [Range(0.02f, 1f)] [SerializeField] private float getdownTimer = .25f; //asagi tusa basma suresi
    private float getdownCounter;

    public bool isGameOver = false;

    public bool isRight = true; //saat yonu mu

    public IconManager rotateIcon;

    public GameObject gameOverPanel;
    
    enum Direction{none, sol, sag, yukari, asagi};

    private Direction suruklemeYonu = Direction.none;
    private Direction suruklemeBitisYonu = Direction.none;

    private float sonrakiDokunmaZamani;
    private float sonrakiSuruklemeZamani;

    [Range(0.05f, 1f)] public float minDokunmaZamani = 0.15f;
    [Range(0.05f, 1f)] public float minSuruklemeZamani = 0.3f;
    private bool isTouch = false;
    
    private void OnEnable()
    {
        TouchManager.DragEvent += Surukle;
        TouchManager.SwipeEvent += SurukleBitti;
        TouchManager.TapEvent += TapFnc;
    }

    private void OnDisable()
    {
        TouchManager.DragEvent -= Surukle;
        TouchManager.SwipeEvent -= SurukleBitti;
        TouchManager.TapEvent -= TapFnc;
    }


    private void Awake()
    {
        board = GameObject.FindObjectOfType<BoardManager>();
        spawner = GameObject.FindObjectOfType<SpawnerManager>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        followShape = GameObject.FindObjectOfType<FollowShapeManager>();
    }

   

    public void StartGame()
    {
        if (spawner)
        {
            spawner.AllShapeNull();

            if (activeShape == null)
            {
                activeShape = spawner.CreateShape();
                activeShape.transform.position = VectorToInt(activeShape.transform.position);
            }

            //gelecek aktif sekli once kucult sonra buyut efekti
            if (activeShape)
            {
                activeShape.transform.localScale = Vector3.zero;
                isMove = false;
                activeShape.transform.DOScale(1, .5f).SetEase(Ease.OutBack).OnComplete(() => isMove = true);
            }


            //eldeki sekli img ye yerlestir
            if (holderShape == null)
            {
                holderShapeImg.GetComponent<CanvasGroup>().DOFade(1, .6f);
                holderShape = spawner.CreateHolderShape();

                //aktif sekil ile eldeki ayni ise tekrar spawn et
                if (holderShape.name == activeShape.name)
                {
                    Destroy(holderShape.gameObject);
                    holderShape = spawner.CreateHolderShape();

                    holderShapeImg.sprite = holderShape.shapeSekil;
                    holderShape.gameObject.SetActive(false);
                }
                else
                {
                    holderShapeImg.sprite = holderShape.shapeSekil;
                    holderShape.gameObject.SetActive(false);
                }


            }
        }

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }

        downLevelCounter = downTimer;
    }

    private void Update()
    {

        if (!board || !spawner || !activeShape || isGameOver || !scoreManager || !isMove)
        {
            return;
        }

        InputController();
    }

    //takip eden sekil geriden gelsin diye lateupdate
    private void LateUpdate()
    {
        if (!board || !spawner || !activeShape || isGameOver || !scoreManager || !followShape || !isMove)
        {
            return;
        }

        if (followShape)
        {
            followShape.CreateFollowShape(activeShape, board);
        }
    }


    private void InputController()
    {
       
        //saga hareket
        if ((Input.GetKey("right") && Time.time > keyPressCounter) || Input.GetKeyDown("right"))
        {
            RightMovement();
        }
        
        //sola hareket
        else if ((Input.GetKey("left") && Time.time > keyPressCounter) || Input.GetKeyDown("left"))
        {
            LeftMovement();
        }

        //rotation
        else if ((Input.GetKeyDown("up") && Time.time > rotationCounter))
        {
            RotateMovement();
        }

        //asagi tusuyla hizli inme
        else if (((Input.GetKey("down") && Time.time > getdownTimer)) || Time.time > downCounter)
        {
            DownMovement();
        }
        
        //Mobilde hareket kismi
        else if ((suruklemeBitisYonu==Direction.sag && Time.time>sonrakiSuruklemeZamani) || 
                 (suruklemeYonu== Direction.sag && Time.time>sonrakiDokunmaZamani)) 
        {
            RightMovement();
            sonrakiDokunmaZamani = Time.time + minDokunmaZamani;
            sonrakiSuruklemeZamani = Time.time + minSuruklemeZamani;
            // suruklemeYonu = Direction.none;
            // suruklemeBitisYonu = Direction.none;
        }
        
        else if ((suruklemeBitisYonu==Direction.sol && Time.time>sonrakiSuruklemeZamani) || 
                 (suruklemeYonu== Direction.sol && Time.time>sonrakiDokunmaZamani)) 
        {
            LeftMovement();
            // suruklemeYonu = Direction.none;
            // suruklemeBitisYonu = Direction.none;
        }
        else if ((suruklemeBitisYonu==Direction.yukari && Time.time>sonrakiSuruklemeZamani) || (isTouch))
        {
            RotateMovement();
            sonrakiSuruklemeZamani = Time.time + minSuruklemeZamani;
            // suruklemeBitisYonu = Direction.none;
        }
        else if (suruklemeYonu==Direction.asagi && Time.time > sonrakiDokunmaZamani)
        {
            DownMovement();
            // suruklemeYonu = Direction.none;
        }

        suruklemeYonu = Direction.none;
        suruklemeBitisYonu = Direction.none;
        isTouch = false;


    }

    private void DownMovement()
    {
        downCounter = Time.time + downLevelCounter;
        getdownCounter = Time.time + getdownTimer;

        if (activeShape)
        {
            activeShape.DownMove();

            if (!board.InCurrentPosition(activeShape))
            {
                if (board.isSpillOut(activeShape))
                {
                    activeShape.UpMove();

                    isGameOver = true;

                    SoundManager.instance.SoundEffectRun(6);

                    if (gameOverPanel)
                    {
                        StartCoroutine(GameOverRoutine());
                    }


                    SoundManager.instance.SoundEffectRun(6);
                }
                else
                {
                    ShapePlaced();
                }
            }
        }
    }

    private void RotateMovement()
    {
        activeShape.TurnRight();
        rotationCounter = Time.time + rotationTimer;


        if (!board.InCurrentPosition(activeShape))
        {
            SoundManager.instance.SoundEffectRun(1);
            activeShape.LeftMove();
        }
        else
        {
            isRight = !isRight;
            if (rotateIcon)
            {
                rotateIcon.IconTurn(isRight);
            }

            SoundManager.instance.SoundEffectRun(3);
        }
    }

    private void LeftMovement()
    {
        activeShape.LeftMove();
        keyPressCounter = Time.time + keyPressTimer;


        if (!board.InCurrentPosition(activeShape))
        {
            SoundManager.instance.SoundEffectRun(1);
            activeShape.RightMove();
        }
        else
        {
            SoundManager.instance.SoundEffectRun(3);
        }
    }

    private void RightMovement()
    {
        activeShape.RightMove();
        keyPressCounter = Time.time + keyPressTimer;

        if (!board.InCurrentPosition(activeShape))
        {
            SoundManager.instance.SoundEffectRun(1);
            activeShape.LeftMove();
        }
        else
        {
            SoundManager.instance.SoundEffectRun(3);
        }
    }

    private void ShapePlaced()
    {
        //aktif sekil varsa gerceklestir
        if (activeShape)
        {
            keyPressCounter = Time.time;
            getdownCounter = Time.time;
            rotationCounter = Time.time;


            activeShape.UpMove();
            activeShape.PlayPlacedEffect();

            board.InGrid(activeShape);
            SoundManager.instance.SoundEffectRun(5);

            //tekrar holder degistirmek icin true
            isHolderShapeChange = true;

            if (spawner)
            {
                activeShape = spawner.CreateShape();

                if (activeShape)
                {
                    activeShape.transform.localScale = Vector3.zero;
                    isMove = false;
                    activeShape.transform.DOScale(1, .5f).SetEase(Ease.OutBack).OnComplete(() => isMove = true);
                }

                holderShape = spawner.CreateHolderShape();

                //aktif sekil ile eldeki ayni ise tekrar spawn et
                if (holderShape.name == activeShape.name)
                {
                    Destroy(holderShape.gameObject);
                    holderShape = spawner.CreateHolderShape();

                    holderShapeImg.sprite = holderShape.shapeSekil;
                    holderShape.gameObject.SetActive(false);
                }

                else
                {
                    holderShapeImg.sprite = holderShape.shapeSekil;
                    holderShape.gameObject.SetActive(false);
                }
            }

            if (followShape)
            {
                followShape.ResetFnc();
            }

            //BoardManager daki IEnumarator fonk erisebilmek icin coroutine e cevirdik
            StartCoroutine(board.CleanAllLine());

            if (board.completedRaw > 0)
            {
                scoreManager.RowScore(board.completedRaw);

                if (scoreManager.isLevelUp)
                {
                    SoundManager.instance.SoundEffectRun(7);

                    //asagi inme suresini hizlandirma
                    downLevelCounter = downTimer - Mathf.Clamp(((float)scoreManager.level - 1) * .1f, 0.05f, 1f);

                    //seviye gecildiginde IEnumerator calissin
                    StartCoroutine(LevelUpRoutine());
                }
                else
                {
                    if (board.completedRaw > 1)
                    {
                        SoundManager.instance.LocalSoundRun();
                    }
                }

                SoundManager.instance.SoundEffectRun(4);
            }
        }

    }


    //Aktif shape in transform degerini tam sayiya yuvarladik,keskinlestirdik
    Vector2 VectorToInt(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }

    //Icon donme yonu
    public void RotationIconDirection()
    {
        isRight = !isRight;
        activeShape.RightRotateBtn(isRight);

        //gecerli sekil board disinda ise
        if (!board.InCurrentPosition(activeShape))
        {
            activeShape.RightRotateBtn(!isRight);
            SoundManager.instance.SoundEffectRun(3);
        }
        else
        {
            if (rotateIcon)
            {
                rotateIcon.IconTurn(isRight);
            }

            SoundManager.instance.SoundEffectRun(2);
        }
    }

    //eldeki sekli aktif sekille yer degistir
    public void ChangeHolderShape()
    {
        if (isHolderShapeChange)
        {
            isHolderShapeChange = false;

            activeShape.gameObject.SetActive(false);
            holderShape.gameObject.SetActive(true);

            holderShape.transform.position = activeShape.transform.position;

            activeShape = holderShape;
        }

        if (followShape)
        {
            followShape.ResetFnc();
        }
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(.2f);
        int counter = 0;

        while (counter < levelUpEffect.Length)
        {
            levelUpEffect[counter].PlayEffect();
            yield return new WaitForSeconds(.1f);

            counter++;
        }
    }

    IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(.2f);

        int counter = 0;

        while (counter < gameOverEffect.Length)
        {
            gameOverEffect[counter].PlayEffect();
            yield return new WaitForSeconds(.1f);

            counter++;
        }

        yield return new WaitForSeconds(1f);

        if (gameOverPanel)
        {
            gameOverPanel.transform.localScale = Vector3.zero;
            gameOverPanel.SetActive(true);

            gameOverPanel.transform.DOScale(1, .5f).SetEase(Ease.OutBack);
        }
    }

    void Surukle(Vector2 suruklemeHareket)
    {
        suruklemeYonu = YonuBelirle(suruklemeHareket);
    }

    void SurukleBitti(Vector2 suruklemeHareket)
    {
        suruklemeBitisYonu = YonuBelirle(suruklemeHareket);
    }

    void TapFnc(Vector2 suruklemeHareket)
    {
        isTouch = true;
    }

    Direction YonuBelirle(Vector2 suruklemeHareket)
    {
        Direction suruklemeYonu=Direction.none;

        if (Mathf.Abs(suruklemeHareket.x)>Mathf.Abs(suruklemeHareket.y))
        {
            suruklemeYonu=(suruklemeHareket.x >= 0) ? Direction.sag: Direction.sol;
        }

        else
        {
            suruklemeYonu = (suruklemeHareket.y >= 0) ? Direction.yukari : Direction.asagi;
        }

        return suruklemeYonu;
    }
    
    
}