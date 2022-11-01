using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Audio : MonoBehaviour
{

    AudioSource source;

    public delegate void FinishDialogue();
    public static event FinishDialogue onFinishDialogue;

    private bool isHeroSoundActive = false;

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
            AudioClip clip = AssetDatabase.LoadAssetAtPath("Assets/Audio/" + v.ClipPath + ".mp3", typeof(AudioClip)) as AudioClip;
            //source.PlayOneShot(clip); UNCOMMENT when clips are ready
            yield return new WaitForSeconds(1); //Change for clip.length
        }

        onFinishDialogue();
    }


    public IEnumerator PlayMagicName(AudioClip[] clips)
    {

        foreach (AudioClip clip in clips)
        {
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }

        onFinishDialogue();
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
