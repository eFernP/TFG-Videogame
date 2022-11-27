using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkMaterial : MonoBehaviour
{
    private float speed = 0.6f;
    private bool isBlinking;

    private float[] MAX_INTENSITIES = { 0.01f, 0.1f, 0.2f, 0.3f };
    //private float currentMaxIntensity = 10f;
    private Material material;

    private int lifes = 4;

    private bool increasing = false;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        Player.onStartInvulnerability += startBlinking;
        Player.onFinishInvulnerability += finishBlinking;
        MazePlayer.onStartInvulnerability += startBlinking;
        MazePlayer.onFinishInvulnerability += finishBlinking;
    }

    void startBlinking()
    {
        lifes--;

        if (lifes == 0)
        {
            material.SetFloat("_FresnelPower", 0);
        }
        else
        {
            isBlinking = true;
            material.SetFloat("_FresnelPower", MAX_INTENSITIES[lifes - 1]);
        }



    }

    void finishBlinking()
    {
        isBlinking = false;
        material.SetFloat("_FresnelPower", MAX_INTENSITIES[lifes - 1]);

    }

    void Update()
    {
        
        if (isBlinking)
        {
            float fresnelPower = material.GetFloat("_FresnelPower");
            Debug.Log(fresnelPower);
            if (increasing)
            {
                material.SetFloat("_FresnelPower", fresnelPower + Time.deltaTime * speed);
            }
            else
            {
                material.SetFloat("_FresnelPower", fresnelPower - Time.deltaTime * speed);
            }

            if (fresnelPower >= MAX_INTENSITIES[lifes - 1])
            {
                increasing = false;
            }
            else if (fresnelPower <= 0)
            {
                increasing = true;
            }
        }

    }

    void OnDisable()
    {
        Player.onStartInvulnerability -= startBlinking;
        Player.onFinishInvulnerability -= finishBlinking;
        MazePlayer.onStartInvulnerability -= startBlinking;
        MazePlayer.onFinishInvulnerability -= finishBlinking;
    }
}