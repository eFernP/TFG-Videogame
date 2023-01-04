using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{

    public GameObject TimerText;
    private TMP_Text displayedTime;

    private float remainingTime;
    private float MINUTES = 5f;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        displayedTime = TimerText.GetComponent<TMPro.TextMeshProUGUI>();
        isActive = true;

        remainingTime = MINUTES * 60;
    }

    public bool hasEnded()
    {
        return !isActive;
    }

    public void resetTimer()
    {
        isActive = true;
        remainingTime = MINUTES * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive == false) return;
        remainingTime -= Time.deltaTime;

        if(remainingTime < 1)
        {
            isActive = false;
        }

        int minutes = Mathf.FloorToInt(remainingTime/60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        displayedTime.text = string.Format("{00:00}:{01:00}", minutes, seconds);
    }
}
