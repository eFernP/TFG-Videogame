using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MagicNameManager : InteractiveObject
{

    private string type = Constants.STONE_TYPE; //TODO: Set after instantiate object

    //GameObject hero;
    //GameObject initialText;
    GameObject sprites;
    Audio AudioManager;

    //private bool hasVisibleText = false;
    private bool hasVisibleSprite = false;

    private static int MAX_HERO_DISTANCE = 5;
    private static int NAME_LENGTH = 3;

    private string[] MagicName = new string[NAME_LENGTH];
    private Sprite[] NameSprites = new Sprite[NAME_LENGTH];

    private MagicNamesStack Names;
    private Player playerManager;
    private PlayerVoiceManager playerVoiceManager;
    private PlayerPoseManager playerPoseManager;

    private float UP_FORCE = 50;
    private float FORWARD_FORCE = 300;

    private bool isFalling = true;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        PlayerVoiceManager.onUseMagic += onUseMagic;
        //hero = GameObject.Find("Hero");
        playerManager = base.hero.GetComponent<Player>();
        playerVoiceManager = base.hero.GetComponent<PlayerVoiceManager>();
        playerPoseManager = base.hero.GetComponent<PlayerPoseManager>();
        AudioManager = base.hero.GetComponent<Audio>();
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
    }


    void GetMagicNameSprites()
    {
        for(int i=0; i< NAME_LENGTH; i++)
        {
            NameSprites[i] = Resources.Load<Sprite>("Sprites/Syllable_" + MagicName[i]);
        }
    }


    //void toggleText(bool value)
    //{
    //    MeshRenderer textMesh = initialText.GetComponent<MeshRenderer>();
    //    textMesh.enabled = value;
    //}


    //void checkText(bool otherConditions)
    //{
    //    float heroDistance = Vector3.Distance(this.transform.position, hero.transform.position);
    //    if (!hasVisibleText && (heroDistance < MAX_HERO_DISTANCE || !hasVisibleSprite)) 
    //    {
    //        hasVisibleText = true;
    //        toggleText(true);
    //    }
    //    if (hasVisibleText && (heroDistance >= MAX_HERO_DISTANCE || hasVisibleSprite))
    //    {
    //        hasVisibleText = false;
    //        toggleText(false);
    //    }
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
        rb.AddForce(base.hero.transform.up * UP_FORCE + base.hero.transform.forward * FORWARD_FORCE, ForceMode.Impulse);
    }

    void onUseMagic()
    {
        string pronouncedName = playerVoiceManager.getPronouncedName();

        if (type + "_" + pronouncedName == this.name)
        {
            int actionId = playerPoseManager.getAction();

            if(actionId == 1)
            {
                Impulse();  
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        base.checkText(!hasVisibleSprite, hasVisibleSprite);

        if (base.hasVisibleText && Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(ShowSyllableSprites());
        }

    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.name == "Floor")
        {
            isFalling = false;
        }

        if (collision.collider.name == "Hero" && isFalling)
        {
            playerManager.handleDamage();
        }


    }

    void OnDisable()
    {
        PlayerVoiceManager.onUseMagic -= onUseMagic;
    }
}
