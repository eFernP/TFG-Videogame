using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkLight : MonoBehaviour
{
    private float speed = 10;
    private bool isBlinking;

    private float[] MAX_INTENSITIES = {  1.25f, 2.5f, 3.75f, 5f };
    //private float currentMaxIntensity = 10f;
    private Light light;

    private int lifes = 4;

    private bool increasing = false;

    void Start()
    {
        light = GetComponent<Light>();
        Player.onStartInvulnerability += startBlinking;
        Player.onFinishInvulnerability += finishBlinking;
    }

    void startBlinking()
    {
        lifes--;

        if (lifes == 0)
        {
            light.intensity = 0;
        }
        else
        {
            isBlinking = true;
            light.intensity = MAX_INTENSITIES[lifes - 1];
        }
    }

    void finishBlinking()
    {
        isBlinking = false;
        light.intensity = MAX_INTENSITIES[lifes - 1];
    }

    void Update()
    {
        if (isBlinking)
        {
            if (increasing)
            {
                light.intensity += Time.deltaTime * speed;
            }
            else
            {
                light.intensity -= Time.deltaTime * speed;
            }

            if (light.intensity >= MAX_INTENSITIES[lifes-1])
            {
                increasing = false;    
            }
            else if (light.intensity == 0)
            {
                increasing = true;   
            }
        }

    }

    void OnDisable()
    {
        Player.onStartInvulnerability -= startBlinking;
        Player.onFinishInvulnerability -= finishBlinking;
    }
}