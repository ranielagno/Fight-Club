﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterController : MonoBehaviour {

    public static bool mvBack = false;
    public static bool mvFwd = false;
    public static bool isAttacking = false;
    public static FighterController instance;
    static Animator anim;
    public Transform enemyTarget;
    public Vector3 direction;
    public int health = 100;
    public Slider playerHealthBar;
    public BoxCollider[] colliders;
    public AudioClip[] audioClips;
    AudioSource audioSource;

    private Vector3 playerPosition;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        playerPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
        SetAllBoxColliders(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("fight_idle")) {
            isAttacking = false;
            direction = enemyTarget.position - this.transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.3f);
            
        }

        if (!isAttacking && GameController.allowMovement) { 
            anim.ResetTrigger("wkBack");
            anim.ResetTrigger("wkFwd");
            anim.ResetTrigger("idle");
            

            if (mvBack) {
                anim.SetTrigger("wkBack");
                SetAllBoxColliders(false);
            } else if(mvFwd) {
                anim.SetTrigger("wkFwd");
                SetAllBoxColliders(false);
            } else {
                anim.SetTrigger("idle");
            }

        } else if(isAttacking)
        {
            SetAllBoxColliders(true);
        }

    }

    private void SetAllBoxColliders(bool state)
    {
        colliders[0].enabled = state;
        colliders[1].enabled = state;
    }

    public void PlayAudio(int clip)
    {
        audioSource.clip = audioClips[clip];
        audioSource.Play();
    }

    public void punch()
    {
        isAttacking = true;
        anim.ResetTrigger("idle");
        anim.SetTrigger("punch");
        PlayAudio(0);
    }

    public void kick()
    {
        isAttacking = true;
        anim.ResetTrigger("idle");
        anim.SetTrigger("kick");
        PlayAudio(1);
    }

    public void react()
    {
        isAttacking = true;
        anim.ResetTrigger("idle");
        anim.SetTrigger("react");
        health = health - 10;
        if (health < 10)
        {
            knockout();
        } else
        {
            anim.ResetTrigger("idle");
            anim.SetTrigger("react");
            PlayAudio(2);
        }
        playerHealthBar.value = health;
    }

    public void knockout()
    {
        anim.SetTrigger("knockout");
        PlayAudio(3);
        health = 100;
        playerHealthBar.value = 100;
        GameController.instance.scoreEnemy();
        GameController.instance.OnScreenPoints();
        GameController.instance.rounds();
        GameController.allowMovement = false;

        if (GameController.enemyScore == 2)
        {
            GameController.instance.DoReset();
        }
        else
        {
            StartCoroutine(ResetCharacters());
        }
    }

    IEnumerator ResetCharacters()
    {
        yield return new WaitForSeconds(4f);
        GameObject[] theClone = GameObject.FindGameObjectsWithTag("Player");
        Transform transform = theClone[5].GetComponent<Transform>();
        transform.position = playerPosition;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        anim.SetTrigger("idle");
        anim.ResetTrigger("knockout");
        GameController.allowMovement = true;
    }
}

