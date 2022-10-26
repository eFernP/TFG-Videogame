using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetUI : MonoBehaviour
{
    GameObject camera;

    public bool isVerticalAxisFixed;
    float parentSizeY;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];

        if (!isVerticalAxisFixed)
        {
            parentSizeY = this.transform.parent.GetComponent<MeshRenderer>().bounds.size.y;
        }
        
    }


    Vector3 getCorrectPosition()
    {
        return this.transform.parent.transform.position + new Vector3(0, parentSizeY + 0.2f, 0);
    }


    void CheckCorrectPosition()
    {
        Vector3 correctPosition = getCorrectPosition();

        if (this.transform.position != correctPosition)
        {
            this.transform.position = correctPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!isVerticalAxisFixed)
        {
            CheckCorrectPosition();
        }

        Vector3 relativePosition = this.transform.position - camera.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePosition);
        this.transform.rotation = rotation;
    }
}