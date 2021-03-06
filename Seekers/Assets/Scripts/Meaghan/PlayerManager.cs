﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerManager : Entity
{

    [Header("Player Exclusives")]
    [Range(0.1f, 1f)]
    [Tooltip("Speed at which the car flashes red.")]
    [SerializeField]
    private float flashSpeed;
    [SerializeField]
    private GameObject leftBumperPreview;
    [SerializeField]
    private GameObject rightBumperPreview;
    [SerializeField]
    private GameObject rearBumperPreview;
    [SerializeField]
    private XboxController controller;
    [SerializeField]
    private ParticleSystem speedBoostPart;
    [SerializeField]
    private ParticleSystem speedBoostPart2;
    [SerializeField]
    private AudioSource collisionSound;


    //Variables 
    private IngameUIScript uiScript;
    private Color defaultColour;
    private GameObject carBody;
    private Renderer renderer;
    private WheelDrive playerMove;
    private bool pressedXButton;
    private Rigidbody rb;
    private bool win;

    //Getter and setter
    public bool Win
    {
        get { return win; }
        set { win = value; }
    }

    // Use this for initialization
    void Awake()
    {
        uiScript = FindObjectOfType<IngameUIScript>();
        defaultColour = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color;
        renderer = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>();
        playerMove = gameObject.GetComponent<WheelDrive>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        bumperSelect = 0;
        hasLowHealth = false;
        pressedXButton = false;
        leftBumper.SetActive(false);
        rightBumper.SetActive(false);
        rearBumper.SetActive(false);
        leftBumperPreview.SetActive(false);
        rightBumperPreview.SetActive(false);
        rearBumperPreview.SetActive(false);
        speedBoostPart.Stop();
        speedBoostPart2.Stop();
    }

    void Update()
    {
         //Respawn check
         if(hits >= maxHits)
         {
             //Set all values back and respawn
             playerMove.AbilityToDrive = false;
             rb.drag = 3.0f;
             hasLowHealth = false;
             Respawn();
         }
         else
         {
             //Playe can drive and store position to respawn at
             playerMove.AbilityToDrive = true;
             PositionTimer();
         }

        Items();

        //Boost
        if (isBoosting)
        {
            //BOOST
            UpdateSpeedBoost();

            //Play the particles
            if (speedBoostPart.isPlaying == false)
            {
                speedBoostPart.Play();
            }
            if (speedBoostPart2.isPlaying == false)
            {
                speedBoostPart2.Play();
            }
        }
        else
        {
            //Stop the particles
            speedBoostPart.Stop();
            speedBoostPart2.Stop();
        }

        //Low health iteration start
        if (hits == maxHits - 1)
        {
            hasLowHealth = true;
        }

        if (hasLowHealth)
        {
            //Start the function
            LowHealth();
        }
        else
        {
            //Set it back to the original colour
            renderer.material.color = defaultColour;
        }
    }

    IEnumerator Flasher()
    {
        //While the player has low health
        while (hasLowHealth)
        {
            //Cycles through the two colours and alters them while in the loop
            renderer.material.color = Color.red;
            yield return new WaitForSeconds(flashSpeed);
            renderer.material.color = defaultColour;
            yield return new WaitForSeconds(flashSpeed);
        }
    }


    void LowHealth()
    {
        //Start flashing
        StartCoroutine(Flasher());
    }

    void Items()
    {
        if (hasItem == true)
        {
            if (hasBumper == true)
            {
                if (XCI.GetButtonDown(XboxButton.LeftBumper, controller))
                {
                    bumperSelect++;

                    //Traverse through item selection
                    if (bumperSelect > 2)
                    {
                        bumperSelect = 0;
                    }
                }



                //IF we haven't used the item
                if (pressedXButton == false)
                {
                    //Preview based on side of the car
                    if (bumperSelect == 0)
                    {
                        leftBumperPreview.SetActive(true);
                        rightBumperPreview.SetActive(false);
                        rearBumperPreview.SetActive(false);
                    }
                    else if (bumperSelect == 1)
                    {
                        leftBumperPreview.SetActive(false);
                        rightBumperPreview.SetActive(true);
                        rearBumperPreview.SetActive(false);
                    }
                    else if (bumperSelect == 2)
                    {
                        leftBumperPreview.SetActive(false);
                        rightBumperPreview.SetActive(false);
                        rearBumperPreview.SetActive(true);
                    }
                }

                if (XCI.GetButtonDown(XboxButton.X, controller))
                {
                    //Stop the preview
                    pressedXButton = true;

                    //Input the selection to the car
                    if (bumperSelect == 0)
                    {
                        leftBumperPreview.SetActive(false);
                        rightBumperPreview.SetActive(false);
                        rearBumperPreview.SetActive(false);
                        Bumper(leftBumper);
                        hasItem = false;
                    }
                    else if (bumperSelect == 1)
                    {
                        leftBumperPreview.SetActive(false);
                        rightBumperPreview.SetActive(false);
                        rearBumperPreview.SetActive(false);
                        Bumper(rightBumper);
                        hasItem = false;
                    }
                    else if (bumperSelect == 2)
                    {
                        leftBumperPreview.SetActive(false);
                        rightBumperPreview.SetActive(false);
                        rearBumperPreview.SetActive(false);
                        Bumper(rearBumper);
                        hasItem = false;
                    }
                }
            }
            else //speed boost
            {
                if (XCI.GetButtonDown(XboxButton.X, controller))
                {
                    //Go fast
                    isBoosting = true;
                    hasItem = false;
                }
            }
        }
        else
        {
            bumperSelect = 0;
            pressedXButton = false;
        }
    }


    private void OnCollisionEnter(Collision a_other)
    {
        if(a_other.transform.tag == ("Bumper"))
        {
            //Play the sound
            collisionSound.PlayOneShot(collisionSound.clip, 0.1f);
        }
    }

    protected override void OnCollectItem(SpawnerScript item)
    {
        //Calls Entity's
        base.OnCollectItem(item);

        //Update the UI
        if (uiScript != null)
        {
            uiScript.SetCollectedItem(item.CurrItem);
        }
    }
}
