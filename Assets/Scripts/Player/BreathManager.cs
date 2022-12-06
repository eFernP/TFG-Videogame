using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BreathManager : MonoBehaviour
{
    public SliderBar breathBar;

    private Player playerManager;
    private PlayerPoseManager playerPoseManager;
    private Animator animator;


    private int MAX_BREATH_VALUE = 50;


    public float GetBreath()
    {
        return breathBar.GetValue();
    }

    public void DecreaseBreath(float value)
    {
        breathBar.SetValue(breathBar.GetValue() - value);
    }


    // Start is called before the first frame update
    void Start()
    {
        playerManager = GetComponent<Player>();
        playerPoseManager = GetComponent<PlayerPoseManager>();
        animator = GetComponent<Animator>();
        breathBar.SetMaxValue(MAX_BREATH_VALUE);
        breathBar.SetValue(MAX_BREATH_VALUE);
    }


    // Update is called once per frame
    void Update()
    {

        float breathValue = breathBar.GetValue();


        if (playerManager.IsMoving && !playerPoseManager.isPosing())
        {

            if (breathValue > 0)
            {
                breathBar.SetValue(breathBar.GetValue() - Time.deltaTime * 2f);

                if (playerManager.hasSlowSpeed())
                {
                    animator.speed = 1f;
                    playerManager.increaseSpeed();
                }

            }
            else
            {
                if (playerManager.hasNormalSpeed())
                {
                    animator.speed = 0.75f;
                    playerManager.decreaseSpeed();
                }
            }

        }
        else
        {

            if (breathValue < MAX_BREATH_VALUE)
            {
                breathBar.SetValue(breathBar.GetValue() + Time.deltaTime * 10);
            }


        }
    }

}




