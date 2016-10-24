using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/* This script was developed by Drennen Dooms. All rights reserved. */

public class Health : MonoBehaviour {

    public float health = 100f;
    public Image visualHealth;

	void Update(){

        // Visual representation of the player's health.
        visualHealth.fillAmount = health / 200f;

        //debugging
        if(health > 100f)
        {
            health = 100f;
        } if(health < 0f) { health = 0f; }


	
	}


}
