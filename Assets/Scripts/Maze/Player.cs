using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MazePlayer : MonoBehaviour
{
    private float speed;
    public float rotationSpeed;
    public Transform cameraTransform;

    private Animator animator;
    private CharacterController characterController;
    private float originalStepOffset;

    //public GameObject[] lifePoints;
    private int lifeCounter;
    private bool isInvulnerable;

    private int LIFE_POINTS = 4;
    public GameObject RestartMenuUI;

    private float SPEED = 11f;


    public delegate void StartInvulnerability();
    public static event StartInvulnerability onStartInvulnerability;

    public delegate void FinishInvulnerability();
    public static event FinishInvulnerability onFinishInvulnerability;

    public delegate void UseMagic();
    public static event UseMagic onUseMagic;

    private int currentPose = 0;

    public BlackScreen BlackScreenScript;

    private bool isTeleporting = false;
    private TeleportUnit currentTeleport;

    public GameObject[] coveredParts;
    public GameObject[] uncoveredParts;
    public GameObject light;

    private bool isCover = false;

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
            animator.SetBool("isDoingPose" + pose, true);
        }
        if (Input.GetKeyUp(keyCode))
        {
            currentPose = 0;
            animator.SetBool("isDoingPose" + pose, false);
        }
    }

    void toggleMesh(GameObject obj, bool enabled)
    {
        SkinnedMeshRenderer mesh = obj.GetComponent<SkinnedMeshRenderer>();
        if(mesh != null)
        {
            mesh.enabled = enabled;
        }
        else
        {
            obj.GetComponent<MeshRenderer>().enabled = enabled;
        }
    }

    void checkCover()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (isCover)
            {
                foreach(GameObject obj in coveredParts)
                {
                    toggleMesh(obj, false);
                }
                foreach (GameObject obj in uncoveredParts)
                {
                    toggleMesh(obj, true);
                }
                isCover = false;
                light.GetComponent<Light>().enabled=true;
            }
            else
            {
                foreach (GameObject obj in coveredParts)
                {
                    toggleMesh(obj, true);
                }
                foreach (GameObject obj in uncoveredParts)
                {
                    toggleMesh(obj, false);
                }
                isCover = true;
                light.GetComponent<Light>().enabled = false;
            }
        }
    }

    public bool getIsCovered()
    {
        return isCover;
    }


    public void handleDamage()
    {
        Debug.Log("INVULNE?" + isInvulnerable);
        if (!isInvulnerable)
        {
            lifeCounter--;
            StartCoroutine(MakeInvulnerable());
        }
    }


    void handleTeleport()
    {
        if (!this.transform.position.Equals(currentTeleport.getDestination()))
        {
            if (BlackScreenScript.getOpacity() < 1)
            {
                Debug.Log("Fade oput");
                BlackScreenScript.fadeOut();
            }
            else
            {
                //TODO: Change also camera rotation/position
                this.transform.position = currentTeleport.getDestination();
                this.transform.rotation = Quaternion.Euler(0, currentTeleport.getDestinationOrientation(), 0);
            }
        }
        else
        {
            if (BlackScreenScript.getOpacity() > 0)
            {
                BlackScreenScript.fadeIn();
            }
            else
            {
                isTeleporting = false;
            }
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        speed = SPEED;
        lifeCounter = LIFE_POINTS;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;

    }


    // Update is called once per frame
    void Update()
    {
        if (lifeCounter == 0)
        {
            FinishGame();
        }


        if (Time.timeScale == 0f)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        else if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (isTeleporting)
        {
            animator.SetBool("isRunning", false);
            handleTeleport();
            return;
        }

        bool isMoving = isPressingMovementKey();
        bool isRunning = animator.GetBool("isRunning");


        //checkPose(1, KeyCode.Mouse0); //ONLY WHEN PLAYER UNLOCKS
        //checkPose(2, KeyCode.Mouse1); //ONLY WHEN PLAYER UNLOCKS
        checkCover();

        if (isMoving && currentPose == 0)
        {
            if (!isRunning)
            {
                animator.SetBool("isRunning", true);
            }
            move();

        }
        else
        {

            if (isRunning)
            {
                animator.SetBool("isRunning", false);
            }

        }
    }


    void FinishGame()
    {
        //RestartMenuUI.SetActive(true); //UNCOMMENT
        Time.timeScale = 0f;
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "TeleportUnit")
        {
            isTeleporting = true;
            currentTeleport= collider.gameObject.GetComponent<TeleportUnit>();
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {

            handleDamage();
        }
    }

}

