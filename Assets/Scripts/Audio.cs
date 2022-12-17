using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Audio : MonoBehaviour
{

    AudioSource source;

    private bool isHeroSoundActive = false;
    private bool isPlayingSoundEffect = false;

    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public IEnumerator PlayHeroSound(AudioClip clip, float waitTime)
    {

        if (!isHeroSoundActive)
        {
            isHeroSoundActive = true;
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(waitTime);
            isHeroSoundActive = false;
        }
    }


    IEnumerator PlayDialogue(Vocals[] vocals)
    {

        foreach(Vocals v in vocals)
        {
            Debug.Log(v.Text);
            AudioClip clip = Resources.Load<AudioClip>("Audio/" + v.ClipPath);
            //source.PlayOneShot(clip); UNCOMMENT when clips are ready
            yield return new WaitForSeconds(1); //Change for clip.length when the clip exists
        }

    }


    public IEnumerator PlayMagicName(AudioClip[] clips)
    {

        foreach (AudioClip clip in clips)
        {
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
    }



    public void StartDialogue(Vocals[] vocals)
    {
        StartCoroutine(PlayDialogue(vocals));
    }


    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
    }


}
