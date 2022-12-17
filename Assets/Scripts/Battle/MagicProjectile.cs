using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    private float Speed = 30f;
    private GameObject Shadow;

    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        Shadow = this.transform.parent.GetChild(1).gameObject;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(0, -Time.deltaTime * Speed, 0);
    }


    void DestroyAll()
    {
        Destroy(gameObject);
        Destroy(Shadow);
        Destroy(this.transform.parent.gameObject);
    }

    IEnumerator Finish(bool isCollisionWithHero)
    {
        if (isCollisionWithHero)
        {
            audio.Play();
        }

        yield return new WaitForSeconds(audio.clip.length);
        Destroy(gameObject);
        Destroy(this.transform.parent.gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.name != "Ceiling")
        {
            StartCoroutine(Finish(collider.name == "Hero"));
        }

        if(collider.name == "Floor")
        {
            Destroy(Shadow);
        }
    }
}
