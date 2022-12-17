using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerVoiceManager : MonoBehaviour
{
    public WarningMessage warning;

    private Audio AudioManager;
    private BreathManager breathManager;

    private static int NAME_LENGTH = 3;
    private int SYLLABLE_BREATH_VALUE = 3;
    private float SECONDS_BETWEEN_SYLLABLES = 0.1f;

    private AudioClip[] NameAudioClips = new AudioClip[NAME_LENGTH];
    private SpriteRenderer GraphemeRenderer;

    public delegate void UseMagic();
    public static event UseMagic onUseMagic;


    private bool hasVisibleSprite = false;

    private List<string> pronouncedName = new List<string>();


    private Dictionary<KeyCode, string> MagicSyllables = new Dictionary<KeyCode, string>()
     {
            {KeyCode.Alpha1, Constants.SYLLABLES[0]},
            {KeyCode.Alpha2, Constants.SYLLABLES[1]},
            {KeyCode.Alpha3, Constants.SYLLABLES[2]},
            {KeyCode.Alpha4, Constants.SYLLABLES[3]},
            {KeyCode.Alpha5, Constants.SYLLABLES[4]},
     };

    private Dictionary<KeyCode, MagicSyllable> MagicNameFiles = new Dictionary<KeyCode, MagicSyllable>();


    public void clearPronouncedName()
    {
        pronouncedName.Clear();
    }


    void getMagicNameFiles()
    {
        int index = 0;
        foreach (KeyValuePair<KeyCode, string> syllable in MagicSyllables)
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/Syllable_" + syllable.Value);
            Sprite grapheme = Resources.Load<Sprite>("Sprites/Syllable_" + syllable.Value);
            MagicNameFiles.Add(syllable.Key, new MagicSyllable(syllable.Value, clip, grapheme));
            index++;
        }

    }

    IEnumerator ShowSprite(KeyCode key, float duration)
    {
        if (!hasVisibleSprite)
        {
            hasVisibleSprite = true;
            GraphemeRenderer.sprite = MagicNameFiles[key].Grapheme;
            yield return new WaitForSeconds(duration);
            GraphemeRenderer.sprite = null;
            hasVisibleSprite = false;
        }

    }


    public string getPronouncedName()
    {
        string name = "";
        int firstIndex = pronouncedName.Count - NAME_LENGTH;
        for (int i = firstIndex; i < pronouncedName.Count; ++i)
        {
            // Do something with list[i]
            if (i == firstIndex)
            {
                name = pronouncedName[i];
            }
            else
            {
                name = name + "_" + pronouncedName[i];
            }

        }
        return name;
    }


    void checkVoice()
    {

        foreach (KeyValuePair<KeyCode, MagicSyllable> syllable in MagicNameFiles)
        {
            if (Input.GetKeyDown(syllable.Key))
            {
                if (breathManager.GetBreath() >= SYLLABLE_BREATH_VALUE)
                {
                    StartCoroutine(ShowSprite(syllable.Key, SECONDS_BETWEEN_SYLLABLES));
                    StartCoroutine(AudioManager.PlayHeroSound(syllable.Value.Clip, SECONDS_BETWEEN_SYLLABLES));

                    breathManager.DecreaseBreath(SYLLABLE_BREATH_VALUE);

                    pronouncedName.Add(syllable.Value.Name);

                    Debug.Log(pronouncedName.Count);
                    if (pronouncedName.Count >= NAME_LENGTH)
                    {
                        Debug.Log("use magic?");
                        onUseMagic();
                    }
                }
                else
                {
                    StartCoroutine(warning.ShowMessage());
                }

            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        AudioManager = GetComponent<Audio>();
        breathManager = GetComponent<BreathManager>();
        GraphemeRenderer = this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        getMagicNameFiles();

    }


    // Update is called once per frame
    void Update()
    {
        checkVoice();
    }



}




