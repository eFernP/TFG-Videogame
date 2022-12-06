using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerPoseManager : MonoBehaviour
{
    private PlayerVoiceManager VoiceManager;
    private Animator animator;

    private int currentPose = 0;

    public bool isPosing()
    {
        return currentPose != 0;
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
            VoiceManager.clearPronouncedName();
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        VoiceManager = GetComponent<PlayerVoiceManager>();
    }


    // Update is called once per frame
    void Update()
    {


        checkPose(1, KeyCode.Mouse0);
        checkPose(2, KeyCode.Mouse1);

    }
}




