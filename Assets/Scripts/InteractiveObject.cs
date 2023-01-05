using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    protected GameObject hero;

    private GameObject initialText;
    protected bool hasVisibleText = false;

    private static int MAX_HERO_DISTANCE = 4;

    // Start is called before the first frame update
    public virtual void Start()
    {
        initialText = this.transform.GetChild(0).gameObject;
        hero = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    public void toggleText(bool value)
    {
        hasVisibleText = value;
        MeshRenderer textMesh = initialText.GetComponent<MeshRenderer>();
        textMesh.enabled = value;
    }

    protected void checkText(bool otherVisibleConditions = false, bool otherInvisibleConditions = false)
    {
        float heroDistance = Vector3.Distance(this.transform.position, hero.transform.position);
        if (!hasVisibleText && (heroDistance < MAX_HERO_DISTANCE || otherVisibleConditions))
        {
            //disableOtherInteractiveObjectTexts();
            toggleText(true);
        }
        if (hasVisibleText && (heroDistance >= MAX_HERO_DISTANCE || otherInvisibleConditions))
        {
            toggleText(false);
        }
    }

}
