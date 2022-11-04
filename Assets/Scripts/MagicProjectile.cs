using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    private float Speed = 30f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(0, -Time.deltaTime * Speed, 0);

        if(this.transform.position.y <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
