using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkEffect : MonoBehaviour
{
    private Color startColor = Color.red;
    private Color endColor = Color.black;
    [Range(0, 10)]
    public float speed = 1;
    private bool isBlinking;

    Image img;

    void Start()
    {
        img = GetComponent<Image>();
        Player.onStartInvulnerability += startBlinking;
        Player.onFinishInvulnerability += finishBlinking;
    }

    void startBlinking()
    {
       
        isBlinking = true;
    }

    void finishBlinking()
    {
        isBlinking = false;
        img.color = startColor;
    }

    void Update()
    {
        if (isBlinking)
        {
            img.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
        }
        else
        {
            img.color = startColor;
        }
        
    }

    void onDisable()
    {
        Player.onStartInvulnerability -= startBlinking;
        Player.onFinishInvulnerability -= finishBlinking;
    }
}