using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public BattleManager BattleScript;
    public SliderBar friendshipBar;
    public AudioClip slash;
    public AudioClip specialSlash;
    public AudioClip swoosh;
    public AudioClip stoneImpact;

    private GameObject hero;
    public float duration;
    private float dissolveValue = 0.01f;

    private float invisibleMeshValue = 1;
    private float visibleMeshValue = 0.01f;

    public GameObject Body;
    public GameObject Head;

    private float initialPosition;
    private int rotationSpeed = 600;
    private float invisibleSpeed = 40f;
    private float speedBeforeCharge = 6.5f;

    private Animator animator;
    private BossStatus status = BossStatus.NotMoving;
    private bool isSpecialAttack = false;
    private bool hasCollided = false;
    private Vector3 targetPosition;
    private float targetDistance;
    private Quaternion facingTargetRotation;

    private int minDistanceForSpecialAttack = 8;


    public GameObject pointer;

    private int specialAttackStep = 0;


    private float ROOM_CENTER_X = 0.5f;
    private float ROOM_CENTER_Z = 25f;


    private int STONE_COLLISION_POINTS = 5;

    private bool battleFinished = false;

    private Audio AudioManager;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(true);
        initialPosition = this.transform.position.x;
        animator = GetComponent<Animator>();
        hero = GameObject.FindGameObjectsWithTag("Player")[0];
        animator.SetBool("isInCombat", false);
        AudioManager = hero.GetComponent<Audio>();

    }


    float getHeroPosition()
    {
        return Vector3.Distance(this.transform.position, hero.transform.position);
    }

    void ChangeDissolveValue(float to)
    {
        dissolveValue = Mathf.MoveTowards(dissolveValue, to, (1 / duration) * Time.deltaTime);

        Material headMaterial = Head.GetComponent<Renderer>().material;
        Material[] bodyMaterials = Body.GetComponentInChildren<SkinnedMeshRenderer>().materials;

        bodyMaterials[0].SetFloat("_DissolveValue", dissolveValue);
        headMaterial.SetFloat("_DissolveValue", dissolveValue);

    }

    void makeVisible()
    {
        if (dissolveValue > visibleMeshValue)
        {
            ChangeDissolveValue(visibleMeshValue);
        }
    }

    void checkSpecialAttack()
    {
        if (!isSpecialAttack)
        {
            float dist = getHeroPosition();


            //A + unit vector of AB * distance
            Vector3 unitVector = Vector3.Normalize(hero.transform.position - this.transform.position);
            Vector3 finalPoint = this.transform.position + (unitVector * (dist+speedBeforeCharge*2.0f));

            RaycastHit hit;
            bool hasObstacle = Physics.Linecast(this.transform.position, finalPoint, out hit);

            

            if (dist > minDistanceForSpecialAttack)
            {

                if (hasObstacle && hit.collider.name != "Floor")
                {
                    targetPosition = this.transform.position + (unitVector * hit.distance); 
                }
                else
                {
                    targetPosition = finalPoint;
                   
                }
                animator.SetBool("isAttackingFast", false);

                isSpecialAttack = true;
                targetDistance = Vector3.Distance(this.transform.position, targetPosition);


                Vector3 relativePosition = targetPosition - this.transform.position;
                facingTargetRotation = Quaternion.LookRotation(relativePosition);
                specialAttackStep = 1;


                //POINTER TO DEBUG
                //GameObject[] pointers = GameObject.FindGameObjectsWithTag("Pointer");

                //foreach (GameObject p in pointers)
                //    GameObject.Destroy(p);

                //Instantiate(pointer, targetPosition, Quaternion.identity);


            }
        }

    }



    bool hasAnimationFinished()
    {
        return !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
    }

    void changeState(BossStatus newStatus, string fromState, string toState)
    {
        if (fromState != null)
        {
            animator.SetBool(fromState, false);
        }

        if (toState != null)
        {
            animator.SetBool(toState, true);
        }

        status = newStatus;
    }


    bool hasToStop()
    {
        float dist = Vector3.Distance(targetPosition, this.transform.position);

        if (dist <= speedBeforeCharge / 2 || dist > targetDistance * 1.5f)
        {
            return true;
        }
        return false;
    }


    void printState()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Prepare")) Debug.Log("Prepare");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Charge")) Debug.Log("Charge");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) Debug.Log("Attack");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("OnGuard")) Debug.Log("OnGuard");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("LiteAttack")) Debug.Log("LiteAttack");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) Debug.Log("Idle");
    }

    public void handleCollision()
    {
        hasCollided = true;

        if (isSpecialAttack)
        {
            AudioManager.PlaySound(specialSlash);
        }
        else
        {
            AudioManager.PlaySound(slash);
        }

       
    }


    void DoTranslation()
    {

        if (specialAttackStep == 1)
        {
            //float angle = Quaternion.Angle(this.transform.rotation, facingTargetRotation);
            //if (angle >= 1)
            //{

            //    status = BossStatus.NotMoving;
            //    this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, facingTargetRotation, rotationSpeed * Time.deltaTime);
            //}
            //else
            //{
            //Debug.Log("FIRST STEP");
            //printState();
            hasCollided = false;
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, facingTargetRotation, rotationSpeed * Time.deltaTime);

            specialAttackStep = 2;

            animator.SetBool("isPreparing", true);
            //}

        }
        else if (specialAttackStep == 2)
        {
            
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, facingTargetRotation, rotationSpeed * Time.deltaTime);

            float angle = Quaternion.Angle(this.transform.rotation, facingTargetRotation);
            if(angle < 1 && hasAnimationFinished())
            {
                AudioManager.PlaySound(swoosh);
                animator.SetBool("isPreparing", false);
                animator.SetBool("isCharging", true);
                specialAttackStep = 3;
            }
        }
        else if (specialAttackStep == 3)
        {
            if(!animator.IsInTransition(0) && (hasCollided || hasToStop()))
            {
                specialAttackStep = 4;
                this.transform.Translate(Vector3.forward * speedBeforeCharge * Time.deltaTime);
                //navMeshAgent.speed = speed;
                //navMeshAgent.acceleration = 10;
                //changeState(BossStatus.Attacking, "isCharging", "isAttacking");
                //Debug.Log("THIRD STEP");
                //printState();
                animator.SetBool("isAttacking", true);
                animator.SetBool("isCharging", false);
            }
            else 
            {

                if (dissolveValue < invisibleMeshValue)
                {
                    ChangeDissolveValue(invisibleMeshValue);
                }


                //navMeshAgent.speed = invisibleSpeed;
                //navMeshAgent.acceleration = invisibleSpeed * 4;
                //navMeshAgent.SetDestination(targetPosition);
                this.transform.Translate(Vector3.forward * invisibleSpeed * Time.deltaTime);
                //Debug.Log(this.transform.rotation + " " + facingTargetRotation);


            }

        }
        else if (specialAttackStep == 4)
        {
            if (!hasAnimationFinished())
            {

                makeVisible();
                this.transform.Translate(Vector3.forward * speedBeforeCharge * Time.deltaTime);
            }
            else
            {
                isSpecialAttack = false;
                hasCollided = false;
                animator.SetBool("isAttacking", false);
                //Debug.Log("LAST STEP");
                //printState();
                //changeState(BossStatus.OnGuard, "isAttacking", null);
                specialAttackStep = 0;

            }

        }

    }



    void doPhase1()
    {

        if (animator.GetBool("isInCombat") == false)
        {
            animator.SetBool("isInCombat", true);
        }

        checkSpecialAttack();

        if (isSpecialAttack)
        {
            DoTranslation();

        }
        else
        {
            float heroPosition = getHeroPosition();
            makeVisible();

            if (heroPosition > 1.5f)
            {
                Quaternion rotation = Quaternion.LookRotation(hero.transform.position - this.transform.position);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                this.transform.Translate(Vector3.forward * speedBeforeCharge * Time.deltaTime);
            }

            if (getHeroPosition() < minDistanceForSpecialAttack/2)
            {
                if (animator.GetBool("isAttackingFast") == false)
                {
                    animator.SetBool("isAttackingFast", true);
                }

            }
            else
            {
                if (animator.GetBool("isAttackingFast") == true)
                {
                    animator.SetBool("isAttackingFast", false);
                }
            }

        }
    }


    void doPhase2()
    {
        animator.SetBool("isAttackingFast", false);
        animator.SetBool("isInCombat", false);


        bool inCenter = this.transform.position.x == ROOM_CENTER_X && this.transform.position.z == ROOM_CENTER_Z;

        if(dissolveValue < invisibleMeshValue && !inCenter)
        {
            ChangeDissolveValue(invisibleMeshValue);
        }
        else if(!inCenter)
        {
            this.transform.position = new Vector3(ROOM_CENTER_X, this.transform.position.y, ROOM_CENTER_Z);
            this.transform.rotation = Quaternion.identity;
        }
        else if(dissolveValue > visibleMeshValue)
        {
            ChangeDissolveValue(visibleMeshValue);
        }
    }

    void leaveRoom()
    {
        if (dissolveValue < invisibleMeshValue)
        {
            ChangeDissolveValue(invisibleMeshValue);
        }
        else
        {
            Debug.Log("FINISH BATTLE");
            this.gameObject.SetActive(false);
            battleFinished = true;
            Debug.Log("BATTLE FINISHED");
        }
    }



    // Update is called once per frame
    void Update()
    {
        int currentPhase = BattleScript.GetCurrentPhase();

        if(currentPhase == -1 && !battleFinished)
        {
            leaveRoom();
        }
        else if (currentPhase == 1 || specialAttackStep != 0)
        {
            doPhase1();
        }
        else if(currentPhase == 2 && specialAttackStep == 0)
        {
            doPhase2();
        }



    }


    void OnTriggerEnter(Collider collider)
    {

        string other = collider.name.Split('_')[0];
        if(other == "Stone" && BattleScript.GetCurrentPhase() == 2)
        {
            friendshipBar.SetValue(friendshipBar.GetValue() + (float)STONE_COLLISION_POINTS);
            AudioManager.PlaySound(stoneImpact);
            //FOR TESTING
            //if(roundForTesting == 2)
            //{
            //    friendshipBar.SetValue(100f);
            //}
            //else
            //{
            //    friendshipBar.SetValue(60f);
            //    roundForTesting++;
            //}

        }

    }

}
