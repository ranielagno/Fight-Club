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
	}
	
	// Update is called once per frame
	void Update () {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("fight_idleCopy"))
        {
            direction = playerTransform.position - this.transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.3f);
        }

        Debug.Log(direction.magnitude);

        if(GameController.allowMovement == false)
        {
            /*
            if (direction.magnitude > 12f)
            {
                anim.SetTrigger("walkFWD");
            }
            else
            {
                anim.ResetTrigger("walkFWD");
            }
            */

            if (direction.magnitude < 12f && direction.magnitude > 8f)
            {
                anim.SetTrigger("kick");
            }
            else
            {
                anim.ResetTrigger("kick");
            }

            if (direction.magnitude <= 8f)
            {
                anim.SetTrigger("punch");
            }
            else
            {
                anim.ResetTrigger("punch");
            }
        }
        

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
        }
        
    }

    public void EnemyKnockout()
    {
        anim.SetTrigger("knockout");
    }
}
