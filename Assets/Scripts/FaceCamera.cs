using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    GameObject camera;


    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 relativePosition = this.transform.position - camera.transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePosition);
        this.transform.LookAt(new Vector3(camera.transform.position.x, this.transform.position.y, camera.transform.position.z));
        //this.transform.rotation = rotation;
    }
}
