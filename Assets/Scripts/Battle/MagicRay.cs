using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MagicRay : MonoBehaviour
{
    //public VisualEffect vfx;


    private float SECONDS = 1.1f;
    private float remainingTime;
    private bool hasTimeEnded = false;
    private AudioSource audioSource;

    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        remainingTime = SECONDS;
        audioSource = this.GetComponent<AudioSource>();
        //vfx.Play();
        //Destroy(this.gameObject, 3f);
    }

    IEnumerator ImpactRay()
    {
        audioSource.PlayOneShot(clip);
        Collider collider = this.GetComponent<Collider>();
        collider.enabled = true;
        yield return new WaitForSeconds(0.3f);
        collider.enabled = false;
        yield return new WaitForSeconds(2.6f);
        remainingTime = SECONDS;
        hasTimeEnded = false;
    }

   // Update is called once per frame
    void Update()
    {
        if (!hasTimeEnded)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                hasTimeEnded = true;
                StartCoroutine(ImpactRay());
                
            }
        }
    }
}
