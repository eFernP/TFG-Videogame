using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDamage : MonoBehaviour
{
    public AudioClip clip;
    public ParticleSystem particles;

    private Material material;
    private Collider collider;
    private AudioSource audioSource;

    private float speed = 0.25f;
    private float MAX_WIDTH = 0.1f;
    private float MIN_WIDTH = -0.05f;
    private int value = 1;
    public bool cooldown = true;


    private float currentWidth;
    // Start is called before the first frame update
    void Start()
    {
        material = this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
        collider = this.GetComponent<Collider>();
        audioSource = this.GetComponent<AudioSource>();
        currentWidth = MIN_WIDTH;
        material.SetFloat("_Width", currentWidth);
        StartCoroutine(waitCooldown());

    }

    IEnumerator waitCooldown()
    {
        collider.enabled = false;
        cooldown = true;
        yield return new WaitForSeconds(2f);
        particles.Play();
        yield return new WaitForSeconds(1f);
        cooldown = false;
        currentWidth = MIN_WIDTH;
        value = 1;
        collider.enabled = true;
        audioSource.PlayOneShot(clip);

    }

    // Update is called once per frame
    void Update()
    {
        if (!cooldown) {
            if (currentWidth < MIN_WIDTH)
            {
                //To loop
                //StartCoroutine(waitCooldown());
                Destroy(this.gameObject);
            }
            else if (currentWidth > MAX_WIDTH)
            {
                value = -1;
            }

            currentWidth = currentWidth + Time.deltaTime * speed * value;
            material.SetFloat("_Width", currentWidth);
        }
    }
}
