using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour {

    public static bool mvBack = false;
    public static bool mvFwd = false;
    public static FighterController instance;
    public Transform enemyTarget;
    Animator anim;

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
