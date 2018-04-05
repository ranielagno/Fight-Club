﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") {
            FighterController.instance.react();
            Debug.Log("HIT Enemy");
        }
    }
}
