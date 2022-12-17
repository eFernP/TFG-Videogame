using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveScreen : InteractiveObject
{
    //public GameObject hero;
    public GameObject Map;
    private GameObject[] screens;

    //private GameObject initialText;
    //private bool hasVisibleText = false;

    //private static int MAX_HERO_DISTANCE = 5;

    public override void Start()
    {
        base.Start();
        screens = GameObject.FindGameObjectsWithTag("Map");
    }

    public bool isTextVisible()
    {
        return base.hasVisibleText;
    }


    void showMap()
    {
        Map.active = true;
    }

    void hideMap()
    {
        Map.active = false;
    }


    bool isSomeTextVisible()
    {
        bool isTextVisible = false;

        foreach (GameObject screen in screens)
        {
            if (screen.GetComponent<InteractiveScreen>().isTextVisible())
            {
                isTextVisible = true;
                break;
            }
        }

        return isTextVisible;
    }

    // Update is called once per frame
    void Update()
    {
        base.checkText();

        if (base.hasVisibleText && Input.GetKeyUp(KeyCode.Space))
        {
            if (Map.active)
            {
                hideMap();
            }
            else
            {
                showMap();
            }
        }


        if (!isSomeTextVisible())
        {
            hideMap();
        }

    }
}
