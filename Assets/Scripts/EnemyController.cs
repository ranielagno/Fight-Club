﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public static EnemyController instance;
    public Transform playerTransform;
    private Vector3 direction;
    static Animator anim;
    public int enemyHealth = 100;
    public Slider enemyHealthBar;
    public BoxCollider[] colliders;
    public AudioClip[] audioClips;
    AudioSource audioSource;

    private Vector3 enemyPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        enemyPosition = transform.position;
        SetAllBoxColliders(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("fight_idleCopy"))
        {
            direction = playerTransform.position - this.transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.3f);
            SetAllBoxColliders(false);
        }

        Debug.Log(direction.magnitude);

        if(GameController.allowMovement)
        {
            
            if (direction.magnitude > 13f)
            {
                anim.SetTrigger("walkFWD");
                SetAllBoxColliders(false);
            }
            else
            {
                anim.ResetTrigger("walkFWD");
            }
            

            if (direction.magnitude < 14f && direction.magnitude > 7f)
            {
                SetAllBoxColliders(true);
                if (!audioSource.isPlaying && !anim.GetCurrentAnimatorStateInfo(0).IsName("roundhouse_kick 2"))
                {
                    PlayAudio(1);
                    anim.SetTrigger("kick");
                }
            }
            else
            {
                anim.ResetTrigger("kick");
            }

            if (direction.magnitude <= 7f && direction.magnitude >= 2f)
            {
                SetAllBoxColliders(true);
                if (!audioSource.isPlaying && !anim.GetCurrentAnimatorStateInfo(0).IsName("cross_punch"))
                {
                    PlayAudio(0);
                    anim.SetTrigger("punch");
                }
            }
            else
            {
                anim.ResetTrigger("punch");
            }

            if (direction.magnitude > 0f && direction.magnitude < 2f)
            {
                anim.SetTrigger("walkBack");
                SetAllBoxColliders(false);
                audioSource.Stop();
            }
            else
            {
                anim.ResetTrigger("walkBack");
            }
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

    public void EnemyReact()
    {
        enemyHealth = enemyHealth - 10;
        enemyHealthBar.value = enemyHealth;

        if (enemyHealth < 10)
        {
            EnemyKnockout();
        } else
        {
            anim.ResetTrigger("idle");
            anim.SetTrigger("react");
            PlayAudio(2);
        }
        
    }

    public void EnemyKnockout()
    {
        anim.SetTrigger("knockout");
        PlayAudio(3);
        enemyHealth = 100;
        enemyHealthBar.value = 100;
        GameController.allowMovement = false;
        GameController.instance.scorePlayer();
        GameController.instance.OnScreenPoints();
        GameController.instance.rounds();

        if(GameController.playerScore == 2)
        {
            GameController.instance.DoReset();
        } else
        {
            StartCoroutine(ResetCharacters());
        }
    }

    IEnumerator ResetCharacters()
    {
        yield return new WaitForSeconds(4f);
        GameObject[] theClone = GameObject.FindGameObjectsWithTag("Enemy");
        Transform transform = theClone[1].GetComponent<Transform>();
        transform.position = enemyPosition;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        GameController.allowMovement = true;
    }
}
