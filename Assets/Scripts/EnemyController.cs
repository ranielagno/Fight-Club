using System;
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

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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

        //Debug.Log(direction.magnitude);

        if(GameController.allowMovement)
        {
            
            if (direction.magnitude > 4f)
            {
                anim.SetTrigger("walkFWD");
                SetAllBoxColliders(false);
            }
            else
            {
                anim.ResetTrigger("walkFWD");
            }
            

            if (direction.magnitude < 5f && direction.magnitude > 2f)
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

            if (direction.magnitude <= 2f && direction.magnitude >= 1f)
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

            if (direction.magnitude > 0f && direction.magnitude < 1f)
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
            anim.SetTrigger("react");
            PlayAudio(2);
        }
        
    }

    public void EnemyKnockout()
    {
        anim.SetTrigger("knockout");
        PlayAudio(3);
        enemyHealth = 100;
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
        enemyHealthBar.value = 100;
        GameObject[] theClone = GameObject.FindGameObjectsWithTag("Enemy");
        Transform transform = theClone[2].GetComponent<Transform>();
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        GameController.allowMovement = true;
    }
}
