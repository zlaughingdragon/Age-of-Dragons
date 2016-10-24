using UnityEngine;
using System.Collections;

/* This script was developed by Drennen Dooms, in association with The Laughing Dragon Development Team. 
    All rights reserved. 2016, October 13. */

[RequireComponent(typeof(Rigidbody))]


public class DragonController : MonoBehaviour {

    

    public Vector3 currentPosition, targetPosition, upAngle;
    public Animation anim;

    public GameObject[] dragonPrefabs = new GameObject[1], destructables = new GameObject[500];

    
    public Transform currentTarget, nextTarget;
    
    public float currentTargetDistance, nextTargetDistance;

    public bool attackPlayer, nearObject;

    private Rigidbody rb;

    void Awake()
    {
        upAngle = new Vector3(-25, 0, 0);

        rb = GetComponent<Rigidbody>();
        currentTarget = GameObject.FindGameObjectWithTag("destructable").transform;
        nextTarget = GameObject.FindGameObjectWithTag("destructable").transform;
    }

    void Start()
    {
        
    }

	void Update(){
        destructables = GameObject.FindGameObjectsWithTag("destructable");

        //if(currentTarget != null)
        //currentTargetDistance = Vector3.Distance(transform.position, currentTarget.position);

        //if(currentTarget.)
        //{
        //    currentTarget = GameObject.FindGameObjectWithTag("dragonWaypoint").transform;
        //}

        if (nearObject)
        {
            Destroy(currentTarget.gameObject);

            int rand = Random.Range(0, destructables.Length);
            nextTarget = destructables[rand].transform;
            nextTargetDistance = Vector3.Distance(transform.position, nextTarget.position);
            currentTarget = nextTarget;
        }
        
        
            ////if the player is closer to nextTarget, set currentTarget to nextTarget
            //    if (currentTargetDistance > nextTargetDistance)
            //    {
            //        currentTarget = nextTarget;
            //    }
            
                rb.AddForce(transform.forward);
        
            
    }

    void FixedUpdate()
    {
        if(currentTargetDistance > 5f)
        {
            transform.LookAt(currentTarget);
        } else if(transform.position.y < 1f) { rb.AddForce(transform.up * .5f, ForceMode.Impulse); }
        else
        {
            transform.LookAt(GameObject.FindGameObjectWithTag("dragonWaypoint").transform);
        }

        

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "destructable")
        {
            nearObject = true;
            //print("found destructable");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "destructable")
        {
            nearObject = false;
        }
    }


}


    


