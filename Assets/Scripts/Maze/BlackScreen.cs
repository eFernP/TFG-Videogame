using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    public float speed;

    private float opacity;
    private Image image;

    private bool isFadingIn;
    private int value = 0;

    private bool isChanging = false;

    // Start is called before the first frame update
    void Start()
    {
        opacity = 0;
        {
            image = this.GetComponent<Image>();

        }

    }


    public void fadeIn()
    {
        isChanging = true;
        value = -1;
    }

    public void fadeOut()
    {
        isChanging = true;
        value = 1;
    }


    public float getOpacity()
    {
        return opacity;
    }

    // Update is called once per frame
    void Update()
    {

        if (isChanging == true)
        {
            opacity = image.color.a + Time.deltaTime * speed * value;
            image.color = new Color(image.color.r, image.color.g, image.color.b, opacity);

            if (opacity <= 0 || opacity >= 1)
            {
                value = 0;
                isChanging = false;

                if(opacity <= 0)
                {
                    opacity = 0;
                }
                else
                {
                    opacity = 1;
                }
            }
        }

    }
}
