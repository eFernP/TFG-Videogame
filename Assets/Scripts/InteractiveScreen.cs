using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveScreen : MonoBehaviour
{
    public GameObject hero;
    public GameObject Map;

    private GameObject initialText;
    private bool hasVisibleText = false;

    private static int MAX_HERO_DISTANCE = 5;

    // Start is called before the first frame update
    void Start()
    {
        initialText = this.transform.GetChild(0).gameObject;
    }

    void toggleText(bool value)
    {
        MeshRenderer textMesh = initialText.GetComponent<MeshRenderer>();
        textMesh.enabled = value;
    }

    void checkText()
    {
        float heroDistance = Vector3.Distance(this.transform.position, hero.transform.position);
        if (!hasVisibleText && heroDistance < MAX_HERO_DISTANCE)
        {
            hasVisibleText = true;
            toggleText(true);
        }
        if (hasVisibleText && heroDistance >= MAX_HERO_DISTANCE)
        {
            hasVisibleText = false;
            toggleText(false);
        }
    }

    void showMap()
    {
        Map.active = true;
    }

    void hideMap()
    {
        Map.active = false;
    }


    // Update is called once per frame
    void Update()
    {
        checkText();

        if (hasVisibleText && Input.GetKeyUp(KeyCode.Space))
        {
            //StartCoroutine(AudioManager.PlayMagicName(NameAudioClips));
            if(Map.active)
            {
                hideMap();
            }
            else
            {
                showMap();
            }
        }

        if (!hasVisibleText)
        {
            hideMap();
        }

    }
}
