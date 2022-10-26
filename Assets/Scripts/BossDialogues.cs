using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BossDialogues : MonoBehaviour
{
    private AudioClip testClip;
    Audio audioManager;
    BossSubtitles bossText;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("Hero").GetComponent<Audio>();
        bossText = GameObject.Find("Subtitle").GetComponent<BossSubtitles>();
        Debug.Log(audioManager);
        testClip = AssetDatabase.LoadAssetAtPath("Assets/Audio/testClip.mp3", typeof(AudioClip)) as AudioClip;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.UpArrow))
        //{
        //    audioManager.Play(testClip);
        //    //bossText.setText("HOLA", testClip.length);

        //}
    }
}
