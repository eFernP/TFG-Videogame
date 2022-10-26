using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Player: MonoBehaviour
{
   
    public float speed;
    public float rotationSpeed;
    public Transform cameraTransform;

    private Animator animator;
    private CharacterController characterController;
    private float originalStepOffset;

    public GameObject[] lifePoints;
    public int lifeCounter;
    private bool isInvulnerable;

    public SliderBar breathBar;
    private int MAX_BREATH_VALUE = 50;
    private static int NAME_LENGTH = 3;

    private AudioClip[] NameAudioClips = new AudioClip[NAME_LENGTH];
    public Sprite[] testSprites;
    private SpriteRenderer GraphemeRenderer;

    Boss boss;
    Audio AudioManager;

    public delegate void StartInvulnerability();
    public static event StartInvulnerability onStartInvulnerability;

    public delegate void FinishInvulnerability();
    public static event FinishInvulnerability onFinishInvulnerability;


    public delegate void UseMagic();
    public static event UseMagic onUseMagic;

    private int currentPose = 0;
    private bool hasVisibleSprite = false;

    private List<string> pronouncedName = new List<string>(NAME_LENGTH);


    private Dictionary<KeyCode, string> MagicSyllables = new Dictionary<KeyCode, string>()
     {
            {KeyCode.I, "A"},
            {KeyCode.O, "E"},
            {KeyCode.P, "I"},
            {KeyCode.K, "O"},
            {KeyCode.L, "U"},
            {KeyCode.BackQuote, "B"},
     };

    private Dictionary<KeyCode, MagicSyllable> MagicNameFiles = new Dictionary<KeyCode, MagicSyllable>();


    void getMagicNameFiles()
    {
        int index = 0;
        foreach (KeyValuePair<KeyCode, string> syllable in MagicSyllables)
        {
            AudioClip clip = AssetDatabase.LoadAssetAtPath("Assets/Audio/Syllable_" + syllable.Value + ".mp3", typeof(AudioClip)) as AudioClip;
            MagicNameFiles.Add(syllable.Key, new MagicSyllable(syllable.Value, clip, testSprites[index]));
            index++;
        }

    }

    bool isPressingMovementKey()
    {
        return Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("s");
    }


    void move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        //animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);

        // Player moves towards camera direction
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();


        characterController.Move(movementDirection * inputMagnitude * speed * Time.deltaTime);
        //  Vector3 rotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime, 0);
        //  this.transform.Rotate(rotation);
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }



    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }


    public void MoveToPosition(Vector3 position)
    {
        characterController.enabled = false;
        transform.position = new Vector3(position.x, 0, position.z);
    
        characterController.enabled = true;
    }

    IEnumerator MakeInvulnerable()
    {
        onStartInvulnerability();
        isInvulnerable = true;
        yield return new WaitForSeconds(2);

        onFinishInvulnerability();
        isInvulnerable = false;
    }

    public float GetBreath()
    {
        return breathBar.GetValue();
    }

    public void DecreaseBreath(float value)
    {
        breathBar.SetValue(breathBar.GetValue() - value);
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
        return String.Join("_", pronouncedName.ToArray());
    }

    public int getAction()
    {
        return currentPose;
    }


     void checkPose(int pose, KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            currentPose = pose;
            animator.SetBool("isRunning", false);
            animator.SetBool("isDoingPose"+pose, true);
        }
        if (Input.GetKeyUp(keyCode))
        {
            currentPose = 0;
            animator.SetBool("isDoingPose"+pose, false);
            pronouncedName.Clear();
        }
    }


    void checkVoice()
    {
        foreach (KeyValuePair<KeyCode, MagicSyllable> syllable in MagicNameFiles)
        {
            if (Input.GetKeyDown(syllable.Key))
            {
                Debug.Log("PRESSED" + syllable.Value.Name);
                StartCoroutine(ShowSprite(syllable.Key, syllable.Value.Clip.length));
                StartCoroutine(AudioManager.PlayHeroSound(syllable.Value.Clip));

                
                pronouncedName.Add(syllable.Value.Name);

                if(pronouncedName.Count == NAME_LENGTH)
                {
                    onUseMagic();
                    pronouncedName.Clear();
                    Debug.Log("ELEMENT COUNT?"+ pronouncedName.Count);
                }

            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        lifeCounter = lifePoints.Length;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        boss = GameObject.Find("Boss").GetComponent<Boss>();

        breathBar.SetMaxValue(MAX_BREATH_VALUE);
        breathBar.SetValue(MAX_BREATH_VALUE);


        AudioManager = GetComponent<Audio>();
        GraphemeRenderer = this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        getMagicNameFiles();

    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = isPressingMovementKey();
        bool isRunning = animator.GetBool("isRunning");
        float breathValue = breathBar.GetValue();

        checkPose(1, KeyCode.Alpha1);
        checkPose(2, KeyCode.Alpha2);
        checkPose(3, KeyCode.Alpha3);


        checkVoice();

        if (isMoving && currentPose == 0)
        {
            if (!isRunning)
            {
                animator.SetBool("isRunning", true);
            }
            move();

            
            if(breathValue > 0)
            {
                breathBar.SetValue(breathBar.GetValue() - Time.deltaTime * 4);
            }

        }
        else
        {

            if (isRunning)
            {
                animator.SetBool("isRunning", false);
            }

            if (breathValue < MAX_BREATH_VALUE)
            {
                breathBar.SetValue(breathBar.GetValue() + Time.deltaTime * 3);
            }


        }
    }


    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("COLLISION" + collider.name);

        if (collider.name == "Boss" || collider.gameObject.tag == "Projectile")
        {
            if (lifeCounter > 0 && !isInvulnerable)
            {
                lifeCounter--;
                Destroy(lifePoints[lifeCounter]);
                StartCoroutine(MakeInvulnerable());
            }


            if(collider.name == "Boss")
            {
                boss.handleCollision();
            }
            else
            {
                Destroy(collider.gameObject);
            }

        }           
        
    }


}




