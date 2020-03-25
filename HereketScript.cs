using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HereketScript : MonoBehaviour
{
    #region Verilenler
    private bool toxunulub, sol, sag, SagSwipe = false, SolSwipe = false;
    public bool detectSwipeOnlyAfterRelease = false, Mode1, Mode2, Mode3;
    public Rigidbody2D Masin;
    private Vector2 fingerDown, fingerUp;
    public float SWIPE_THRESHOLD = 20f;
    public float geridonus = 1.0f,DonusSureti=0;
    ColDet Coldet;
    Vector3 positionx;


    #endregion

   
    void Start()
    {
       
        Mode1 = true;
        Mode2 = false;
        Mode3 = false;

        #region ButtonStart
        sol = false;
        sag = false;
        #endregion
        LoadPlayer();
    }
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer()

    {
        PlayerData data = SaveSystem.LoadPlayer();
        Mode1 = data.Mod1;
        Mode2 = data.Mod2;
        Mode3 = data.Mod3;
         
    }

    private void LateUpdate()
    {
        DonusSureti = Coldet.DonusSureti;
    }
    void Update()
    {

        positionx = Masin.transform.position;
        positionx.x = Mathf.Clamp(positionx.x, 1.3f, 4.5f);
        Masin.transform.position = positionx;
        Masin.velocity=new Vector2(DonusSureti*200 * Time.deltaTime, 700 * Time.deltaTime);

        #region AccelometerMode
        if (Mode3==true&& Mode2 == false && Mode1 == false)
        {

            DonusSureti = Input.acceleration.x*2;
        }
        #endregion
        #region ButtonEventTriggerlerUpdate
        if (Mode2 == false && Mode1 == true&& Mode3==false) {
            if (sol == true && sag == false)
        {
                DonusSureti = -1;

            }
        if (sag == true && sol == false)
        {
                DonusSureti = 1;
            }
        if (sag == false && sol == false)
        {
                DonusSureti = 0;
            }
        if (sag == true && sol == true)
        {
                DonusSureti = 0;
            }
    }
        #endregion
        #region SwipeUpdate
        if (Mode2 == true && Mode1 == false&& Mode3==false)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUp = touch.position;
                    fingerDown = touch.position;
                }

                //Detects Swipe while finger is still moving
                if (touch.phase == TouchPhase.Moved)
                {
                    if (!detectSwipeOnlyAfterRelease)
                    {
                        fingerDown = touch.position;
                        checkSwipe();
                    }
                }

                //Detects swipe after finger is released
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            if (Input.touchCount > 0 && SolSwipe == true)
            {

                DonusSureti = -1;
            }

            else if (Input.touchCount > 0 && SagSwipe == true)
            {
                DonusSureti = 1;
            }
            else
            {
                DonusSureti = 0;
                SolSwipe = false;
                SagSwipe = false;
            }
        }
        #endregion
        
    }
    #region ModeVoidleri
    public void Mod1()
    {
        Mode1 = true;
        Mode2 = false;
        Mode3 = false;
        SavePlayer();
    }
    public void Mod2()
    {
        Mode1 = false;
        Mode2 = true;
        Mode3 = false;
        SavePlayer();
    }
    public void Mod3()
    {
        Mode1 = false;
        Mode2 = false;
        Mode3 = true;
        SavePlayer();
    }

    #endregion
    #region ButtonVoidler
    public void SolGiris()
    {
        sol = true;
    }
    public void SagGiris()
    {
        sag = true;
    }
    public void SolCixis()
    {
        sol = false;

    }
    public void SagCixis()
    {
        sag = false;
    }
    #endregion
    #region SwipeVoidler
    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void OnSwipeUp()
    {
        //   Debug.Log("Swipe UP");
        //   obyect.velocity = (Vector2.up * 100 * Time.deltaTime);
    }

    void OnSwipeDown()
    {
        //  Debug.Log("Swipe Down");
    }

    void OnSwipeLeft()
    {
        SolSwipe = true;
        // Debug.Log("Swipe Left");

    }

    void OnSwipeRight()
    {
        SagSwipe = true;
        //  Debug.Log("Swipe Right");



    }
    #endregion
   
}
