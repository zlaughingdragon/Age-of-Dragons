using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/* This script was developed by Drennen Dooms. All rights reserved. */

public class FloatyIndicators : MonoBehaviour {

    Vector3 rot;
	

	void FixedUpdate(){
        
        transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);
        
    }
    void LateUpdate()
    {
        rot = transform.eulerAngles;
        rot.x = 0;
        rot.z = 0;
       
        transform.eulerAngles = rot;
    }


}
