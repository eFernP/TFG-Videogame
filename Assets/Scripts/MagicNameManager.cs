using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MagicNameManager : MonoBehaviour
{

    private string type = Constants.STONE_TYPE; //TODO: Set after instantiate object

    GameObject hero;
    GameObject initialText;
    GameObject sprites;
    Audio AudioManager;

    private bool hasVisibleText = false;
    private bool hasVisibleSprite = false;

    private static int MAX_HERO_DISTANCE = 5;
    private static int NAME_LENGTH = 3;

    private string[] MagicName = new string[NAME_LENGTH];
    private Sprite[] NameSprites = new Sprite[NAME_LENGTH];

    public Sprite[] graphemeSprites;

    private MagicNamesStack Names;
    private Player HeroScript;

    private float UP_FORCE = 50;
    private float FORWARD_FORCE = 300;

    private bool isFalling = true;

    //private Dictionary<string, Sprite> SyllableSprites = new Dictionary<string, Sprite>()
    // {
    //        {Constants.SYLLABLES[0], graphemeSprites[0]},
    //        {Constants.SYLLABLES[1], graphemeSprites[1]},
    //        {Constants.SYLLABLES[2], graphemeSprites[2]},
    //        {Constants.SYLLABLES[3], graphemeSprites[3]},
    //        {Constants.SYLLABLES[4], graphemeSprites[4]},
    //        {Constants.SYLLABLES[5], graphemeSprites[5]},
    // };


    // Start is called before the first frame update
    void Start()
    {
        Player.onUseMagic += onUseMagic;
        hero = GameObject.Find("Hero");
        HeroScript = hero.GetComponent<Player>();
        AudioManager = hero.GetComponent<Audio>();
        initialText = this.transform.GetChild(0).gameObject;
        sprites = this.transform.GetChild(1).gameObject;
        GetMagicName();
        GetMagicNameSprites();

    }


    void GetMagicName()
    {
        Names = GameObject.FindGameObjectsWithTag("SceneManager")[0].GetComponent<MagicNamesStack>();
        string StoneName = Names.GetStoneName();
        MagicName = StoneName.Split('_');

        this.name = type + "_" + StoneName;
        //Debug.Log("MAGIC NAME:" + MagicName[0] + MagicName[1] + MagicName[2]);
        //Debug.Log(Constants.GetStoneName());
        //Debug.Log(Constants.GetStoneName());
    }


    void GetMagicNameSprites()
    {
        for(int i=0; i< NAME_LENGTH; i++)
        {
            //NameAudioClips[i] = Resources.Load<AudioClip>("Audio/Syllable_" + MagicName[i]);
            //NameSprites[i] = AssetDatabase.LoadAssetAtPath("Assets/UI/Sprites/Syllable_" + MagicName[i], typeof(Sprite)) as Sprite; NOT WORKING

            NameSprites[i] = Array.Find(graphemeSprites, element => element.name == "Syllable_"+MagicName[i]);
        }
    }


    void toggleText(bool value)
    {
        MeshRenderer textMesh = initialText.GetComponent<MeshRenderer>();
        textMesh.enabled = value;
    }


    void checkText()
    {
        float heroDistance = Vector3.Distance(this.transform.position, hero.transform.position);
        if (!hasVisibleText && (heroDistance < MAX_HERO_DISTANCE || !hasVisibleSprite))
        {
            hasVisibleText = true;
            toggleText(true);
        }
        if (hasVisibleText && (heroDistance >= MAX_HERO_DISTANCE || hasVisibleSprite))
        {
            hasVisibleText = false;
            toggleText(false);
        }
    }

    //public IEnumerator ShowSyllableSprites()
    //{
    //    hasVisibleSprite = true;
    //    hasVisibleText = false;
    //    for (int i = 0; i < NAME_LENGTH; i++)
    //    {
    //        SpriteRenderer rend = syllableSprite.GetComponent<SpriteRenderer>();
    //        rend.sprite = NameSprites[i];
    //        yield return new WaitForSeconds(NameAudioClips[i].length);
    //    }
    //    syllableSprite.GetComponent<SpriteRenderer>().sprite = null;
    //    hasVisibleSprite = false;
    //    hasVisibleText = true;
    //}


    IEnumerator ShowSyllableSprites()
    {
        hasVisibleSprite = true;
        hasVisibleText = false;
        for (int i = 0; i < NAME_LENGTH; i++)
        {
            SpriteRenderer rend = sprites.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
            rend.sprite = NameSprites[i];
            
        }
        yield return new WaitForSeconds(2);
        for (int i = 0; i < NAME_LENGTH; i++)
        {
            SpriteRenderer rend = sprites.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
            rend.sprite = null;

        }
        hasVisibleSprite = false;

        checkText();
    }

    void Impulse()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        //rb.AddForce(this.transform.forward * 30+this.transform.up*1, ForceMode.Impulse);
        rb.AddForce(hero.transform.up * UP_FORCE + hero.transform.forward * FORWARD_FORCE, ForceMode.Impulse);
    }

    void onUseMagic()
    {
        string pronouncedName = HeroScript.getPronouncedName();
        Debug.Log("PRONOUNCED" + pronouncedName);
        if (type + "_" + pronouncedName == this.name)
        {
            int actionId = HeroScript.getAction();

            if(actionId == 1)
            {
                Impulse();  
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        checkText();

        if (hasVisibleText && Input.GetKeyUp(KeyCode.Space))
        {
            //StartCoroutine(AudioManager.PlayMagicName(NameAudioClips));
            StartCoroutine(ShowSyllableSprites());
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Impulse();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);

        if (collision.collider.name == "Floor")
        {
            isFalling = false;
        }

        if (collision.collider.name == "Hero" && isFalling)
        {
            HeroScript.handleDamage();
        }


    }

    void OnDisable()
    {
        Player.onUseMagic -= onUseMagic;
    }
}
