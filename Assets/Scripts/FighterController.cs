using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour {

    public static bool mvBack = false;
    public static bool mvFwd = false;
    public static bool isAttacking = false;
    public static FighterController instance;
    static Animator anim;
    public Transform enemyTarget;
    public Vector3 direction;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
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

            if (mvBack) {
                anim.SetTrigger("wkBack");
            } else if(mvFwd) {
                anim.SetTrigger("wkFwd");
            } else {
                anim.SetTrigger("idle");
            }

        }

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
    }

}
