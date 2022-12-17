using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    private Vector3 destination;
    private Vector3 origin;

    private Coroutine co;

    private float STOP_TIME = 5f;
    private float SLOW_SPEED = 2f;
    private float FAST_SPEED = 12f;
    private float ROTATION_SPEED = 1000f;

    private float UNCOVERED_PLAYER_DISTANCE = 15f;
    private float COVERED_PLAYER_DISTANCE = 4f;
    private float ATTACK_DISTANCE = 2.5f;

    private GameObject Player;

    private bool isHunting = false;
    private bool foundPlayer = false;
    private bool hasReachedDestination = false;



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        agent.speed = SLOW_SPEED;
    }

    public Vector3 Origin
    {
        get
        {
            return origin;
        }
        set
        {
            origin = value;
        }
    }



    public Vector3 Destination
    {
        get
        {
            return destination;
        }
        set
        {
            destination = value;
            this.GetComponent<NavMeshAgent>().SetDestination(destination);
        }
    }

    float getHeroPosition()
    {
        return Vector3.Distance(this.transform.position, Player.transform.position);
    }

    IEnumerator changeDestination()
    {
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(STOP_TIME);
        Vector3 temp = origin;
        origin = destination;
        destination = temp;
        agent.SetDestination(destination);
        animator.SetBool("isMoving", true);
        hasReachedDestination = false;  

    }

    bool hasAnimationFinished()
    {
        return !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
    }

    public void finishAttack()
    {
        animator.SetBool("isAttacking", false);
        foundPlayer = false;
        isHunting = false;
        
            
       if(getHeroPosition() >= ATTACK_DISTANCE)
        {
            agent.Resume();
            agent.SetDestination(destination);
        }

    }

    void huntPlayer()
    {
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        //{
        //    Debug.Log("FINISH HUNT");
        //    foundPlayer = false;
        //    isHunting = false;
        //    agent.speed = SLOW_SPEED;
        //    animator.SetBool("isMoving", true);
        //    agent.Resume();
        //    agent.SetDestination(destination);

        //    return;
        //}

        if (getHeroPosition() < ATTACK_DISTANCE && !foundPlayer)
        {
            foundPlayer = true;
            animator.SetBool("isAttacking", true);
            //agent.Stop();
        }

        if (!foundPlayer)
        {
            //agent.SetDestination(Player.transform.position);
            this.transform.Translate(Vector3.forward * FAST_SPEED * Time.deltaTime);
        }

        Quaternion rotation = Quaternion.LookRotation(Player.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotation, ROTATION_SPEED * Time.deltaTime);


        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //Debug.Log("attacking" + animator.GetBool("isAttacking"));
        //Debug.Log("moving" + animator.GetBool("isMoving"));
        //Debug.Log("transition??" + animator.IsInTransition(0));
        //if (animator.GetBool("isAttacking") && hasAnimationFinished())
        //{
        //    Debug.Log("FINISH ANIMATION");
        //    isHunting = false;
        //    agent.speed = SLOW_SPEED;
        //    this.gameObject.SetActive(false);
        //    //animator.SetBool("isAttacking", false);
        //    //animator.SetBool("isMoving", true);
        //}

    }

    void checkHunt()//TODO: Check player also if has light turned on
    {
        bool isPlayerCovered = Player.GetComponent<Player>().getIsCovered();
        float playerDistance = isPlayerCovered ? COVERED_PLAYER_DISTANCE : UNCOVERED_PLAYER_DISTANCE;

        if(!isHunting && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && getHeroPosition() < playerDistance)
        {
            RaycastHit hit;
            bool hasObstacle = Physics.Linecast(this.transform.position, Player.transform.position, out hit);

            if (!hasObstacle || hit.collider.name == "Hero")
            {
                isHunting = true;
                agent.Stop();
                if(co != null)
                {
                    StopCoroutine(co);
                }
                
                hasReachedDestination = false;
                if (!animator.GetBool("isMoving"))
                {
                    animator.SetBool("isMoving", true);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (destination == null) return;

        checkHunt();

        if (isHunting)
        {
            huntPlayer();
        }

        if (!agent.pathPending && !isHunting)
        {
            if (!hasReachedDestination && this.transform.position.x == destination.x && this.transform.position.z == destination.z )
            {
                hasReachedDestination = true;
                co = StartCoroutine(changeDestination());
            }
            //else
            //{
            //    Debug.Log("to real destination");
            //    agent.SetDestination(destination);
            //}
        }


    }
}




//StopCoroutine(co); // stop the coroutine
