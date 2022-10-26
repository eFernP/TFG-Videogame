using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    Boss boss;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<Boss>();
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("WALL HIT " + collider.name);
        if (collider.name == "Boss")
        {
            boss.handleCollision();
        }
    }
}
