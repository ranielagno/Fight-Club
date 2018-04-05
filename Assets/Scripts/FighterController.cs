using System.Collections;
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

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
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

        if (!isAttacking) { 
            anim.ResetTrigger("wkBack");
            anim.ResetTrigger("wkFwd");
            anim.ResetTrigger("idle");
            SetAllBoxColliders(false);

            if (mvBack) {
                anim.SetTrigger("wkBack");
            } else if(mvFwd) {
                anim.SetTrigger("wkFwd");
            } else {
                anim.SetTrigger("idle");
            }

        } else
        {
            SetAllBoxColliders(true);
        }

    }

    private void SetAllBoxColliders(bool state)
    {
        colliders[0].enabled = state;
        colliders[1].enabled = state;
    }

    public void punch()
    {
        isAttacking = true;
        anim.ResetTrigger("idle");
        anim.SetTrigger("punch");
    }

    public void kick()
    {
        isAttacking = true;
        anim.ResetTrigger("idle");
        anim.SetTrigger("kick");
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
        }
        playerHealthBar.value = health;
    }

    public void knockout()
    {
        anim.SetTrigger("knockout");
    }
}
