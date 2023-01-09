using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Player: MonoBehaviour
{
    private float speed;
    public float rotationSpeed;
    public Transform cameraTransform;

    public Timer mazeTimer;

    public AudioClip mazeDeathClip;
    public AudioClip biteClip;

    private Animator animator;
    private CharacterController characterController;

    private Boss boss;
    public BattleManager battleManager;

    //public GameObject[] lifePoints;
    private int lifeCounter;
    private bool isInvulnerable;

    private int LIFE_POINTS = 4;
    public GameObject RestartMenuUI;

    private float SPEED = 11f;
    private float SLOW_SPEED = 8f;
    private float STEALTH_SPEED = 4f;
    private float GRAVITY = 9.8f;

    private float verticalSpeed = 0;

    public delegate void StartInvulnerability();
    public static event StartInvulnerability onStartInvulnerability;

    public delegate void FinishInvulnerability();
    public static event FinishInvulnerability onFinishInvulnerability;

    public BlackScreen BlackScreenScript;
    private PlayerPoseManager playerPoseManager;
    private Audio audioManager;

    private TeleportUnit currentTeleport;

    public GameObject[] coveredParts;
    public GameObject[] uncoveredParts;
    public GameObject originalHead;
    public GameObject modifiedHead;
    public GameObject light;

    private bool isTeleporting = false;
    private bool isTeleportingToBattle = false;
    private bool isCover = false;
    private bool isInsideMaze = false;
    private bool isInStealth = false;
    private bool isDyingByMaze = false;

    public bool IsMoving
    {
        get
        {
            return isPressingMovementKey();
        }
    }

    public bool IsInStealth
    {
        get
        {
            return isInStealth;
        }
    }

    public bool hasNormalSpeed()
    {
        return speed == SPEED;
    }

    public bool hasSlowSpeed()
    {
        return speed == SLOW_SPEED;
    }

    public void increaseSpeed()
    {
        speed = SPEED;
    }

    public void decreaseSpeed()
    {
        speed = SLOW_SPEED;
    }

    public void stopMovement()
    {
        speed = 0;
    }

    bool isPressingMovementKey()
    {
        return Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("s");
    }


    void move(Vector3 movementDirection)
    {
        if (characterController.isGrounded) verticalSpeed = 0;
        verticalSpeed -= GRAVITY * Time.deltaTime * 10; //if it is not multiplied by 10, the fall is too slow
        Vector3 velocity = movementDirection * 1 * speed;
        velocity.y = verticalSpeed;

        if (velocity.x != 0 || velocity.y != 0 || velocity.z != 0)
        {
            characterController.Move(velocity * Time.deltaTime);
        }
    

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }


    Vector3 getForwardVelocity()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        //Clamp01 returns a value between 0 and 1. If the given value is greater than 1, returns 1, and if it is negative, a 0 is returned
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        // Player moves towards camera direction
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        return movementDirection;

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


    void checkStealth(){
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isInStealth = !isInStealth;
        }
    }

    public bool getIsCovered()
    {
        return isCover;
    }


    public void handleDamage()
    {
        if (!isInvulnerable)
        {
            lifeCounter--;
            StartCoroutine(MakeInvulnerable());
        }
    }

    public void changeEye()
    {
        toggleMesh(originalHead, false);
        toggleMesh(modifiedHead, true);
    }



    void handleMazeDeath()
    {
        if (!isDyingByMaze) return;

        if (BlackScreenScript.getOpacity() < 1)
        {
            BlackScreenScript.fadeOut();
        }
        else
        {
            FinishGame();
        }
    }


    void handleTeleport()
    {
        if (!this.transform.position.Equals(currentTeleport.getDestination()))
        {
            if (BlackScreenScript.getOpacity() < 1)
            {
                BlackScreenScript.fadeOut();
            }
            else
            {
                //TODO: Change also camera rotation/position
                if(isTeleportingToBattle){
                    SceneManager.LoadScene("BattleScene");
                }else{
                    this.transform.position = currentTeleport.getDestination();
                    this.transform.rotation = Quaternion.Euler(0, currentTeleport.getDestinationOrientation(), 0);
                }
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
                isInsideMaze = currentTeleport.hasMazeDestination;
            }
        }

    }

    public bool getIsInsideMaze()
    {
        return isInsideMaze;
    }


    void FinishGame()
    {
        RestartMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        speed = SPEED;
        lifeCounter = LIFE_POINTS;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        playerPoseManager = GetComponent<PlayerPoseManager>();
        audioManager = GetComponent<Audio>();

        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "BattleScene")
        {
            boss = GameObject.Find("Boss").GetComponent<Boss>();
        }

        SaveData saveData = SaveSystem.LoadGame();

        if(saveData != null){
            if(currentScene.name == "MazeScene")
            {
                this.transform.position = new Vector3(saveData.Position[0], saveData.Position[1], saveData.Position[2]);
                characterController.transform.position = new Vector3(saveData.Position[0], saveData.Position[1], saveData.Position[2]);
            }
            changeEye();
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (mazeTimer != null && mazeTimer.hasEnded() && isInsideMaze && !isDyingByMaze)
        {
            isDyingByMaze = true;
            audioManager.PlaySound(mazeDeathClip);
        }

        handleMazeDeath();

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

        checkCover();

        checkStealth();


        Vector3 movementDirection = Vector3.zero;

        if (isPressingMovementKey() && (playerPoseManager == null || !playerPoseManager.isPosing()))
        {

            if (isInStealth && isRunning)
            {
                animator.SetBool("isRunning", false);
            }

            if (isInStealth && !animator.GetBool("isInStealth"))
            {
                animator.SetBool("isInStealth", true);
                speed = STEALTH_SPEED;
            }

            if (!isInStealth && animator.GetBool("isInStealth"))
            {
                animator.SetBool("isInStealth", false);
                speed = STEALTH_SPEED;
            }

            if (!isInStealth && !isRunning)
            {
                animator.SetBool("isRunning", true);
                speed = SPEED;
            }

            movementDirection = getForwardVelocity();
        }
        else
        {
            if (isRunning)
            {
                animator.SetBool("isRunning", false);
            }

            if (animator.GetBool("isInStealth"))
            {
                animator.SetBool("isInStealth", false);
                speed = SPEED;
            }
        }

        move(movementDirection);
    }



    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Teleport")
        {
            isTeleporting = true;
            if(collider.gameObject.name == "BossTeleportUnit"){
                isTeleportingToBattle = true;
            }else{
                currentTeleport= collider.gameObject.GetComponent<TeleportUnit>();
            }
        }
        if (collider.gameObject.tag == "Enemy")
        {
            audioManager.PlaySound(biteClip);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "MagicDamage" || collider.gameObject.tag == "Enemy" || (collider.name == "Boss" && battleManager.GetCurrentPhase() == 1))
        {

            if (collider.name == "Boss" && !isInvulnerable)
            {
                boss.handleCollision();
            }
            handleDamage();
        }
    }


}

