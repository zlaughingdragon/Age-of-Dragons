using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/* This script was developed by Drennen Dooms. All rights reserved. 

    this script attaches to the player and is the "database" so-to-speak of the resources the player has stored on-person.

    PERSONAL NOTE: Check out VSO, Git, etc.
*/

public class Craftables : MonoBehaviour {

    public Text woodTxt, stoneTxt, foodTxt, progressBarStatus;

    public int resourceAmountToAdd, woodAmount, stoneAmount, foodAmount;
    
    public int ID, timeToWait;

    public bool getResource, gettingResource, canGetResource, timerRunning, timerDone, cancel;

    public GameObject tmp = null;
    List<GameObject> resourceHits = new List<GameObject>();
    
    enum itemType
    {
        wood, stone, food
    }


    static itemType _type = itemType.wood;

    //itemType type = itemType.wood;
    static int _id = 0, itemCount = 0;

    static List<itemType> items = new List<itemType>();

   // public List<GameObject> nearbyResources = new List<GameObject>();

    public ArrayList item = new ArrayList { _type, _id, itemCount };

    public Image radialTimerImg;

    public float timer = 0.0f;

    public Collider[] hitColliders;
    //public Collider[] nearbyResources;
    public List<CraftableResource> nearbyResources;
    /*

        DEVELOPER'S NOTE:
        Please do not forget to fix the multiple-resource bug, induced by standing near more than one
        resource at a time. This results in only being able to get one of the resources until the player
        steps away from the resource and then back, resetting the trigger. This might be achieved
        by creating a list of nearby resources in the CraftableResource script.

    */



    #region conceptScript

    void Start()
    {
        progressBarStatus.text = ""; 


    }

    void Update()
    {

        woodTxt.text = woodAmount.ToString("F0");
        stoneTxt.text = stoneAmount.ToString("F0");
        foodTxt.text = foodAmount.ToString("F0");

        /*
        if (resourceHits.Count < 1 && Input.GetButtonDown("GrabResource")) { print("no objects nearby."); }
        else
                if (resourceHits.Count < 2)
        {

            int i = 0;

            while (i < hitColliders.Length)
            {
                nearbyResources.Add(hitColliders[i].gameObject);
                i++;
            }
            */

        hitColliders = Physics.OverlapSphere(gameObject.transform.position, 0.5f);//.Where(x => x.GetType() == typeof(CraftableResource)).ToArray();

        
        if (Input.GetButtonDown("GrabResource"))
        {
            nearbyResources = hitColliders.Where(x => x.GetComponentInParent<CraftableResource>()).Select(x => x.GetComponentInParent<CraftableResource>()).ToList();

            //int i = 0;
            //     while (i < hitColliders.Length)
            // {

            //     if (hitColliders[i].gameObject.tag == "resource")
            //     {
            //         nearbyResources[i] = hitColliders[i].gameObject;
            //     }
            //         i++;
            // }

            for (int i = 0; i < hitColliders.Length; i++)
            {
               
            }
            if (nearbyResources != null)
            {
                if (nearbyResources.Count < 1)
                {
                    print("No Objects.");
                }
                else if (nearbyResources.Count > 1)
                {
                    print("Too many objects.");
                }
                else
                {
                    if (!canGetResource)
                    {
                        StartCoroutine("ResourceCollectError", "Wait for resource cooldown.");
                    }
                    else
                    {
                        StartCoroutine("GetResourceTimer");
                    }

                    if (!timerRunning && timer == 0f)
                    {
                        StartCoroutine("ResourceCollectError", "Failed to interact; please try again.");
                    }

                }


                if (getResource && gettingResource == false)
                {

                    StartCoroutine("GetResource");

                }

            }

        }
        
        if (timer < 8f && GetComponent<PlayerController>().currentSpeed > 0.2f)
        {

            timerRunning = false;
            getResource = false;
            timerDone = true;
          //  cancel = true;
            StopCoroutine(GetResourceTimer());

        }

    }

    void FixedUpdate()
    {
       

        radialTimerImg.fillAmount = timer / 5f;

        if (timerRunning)
        { timer += Time.deltaTime;
            progressBarStatus.text = ("Collecting " + (itemType)item[0] + ".");
            canGetResource = false;
        }
        else
        {
            if(timer > 0f)
            {
                timer -= Time.deltaTime * 1.8f;
                canGetResource = false;
            } else { canGetResource = true; }
        }
        
     

        if(timer > timer + timeToWait)
        {
            timerDone = true;
        }
        else { timerDone = false;  }
    }

    IEnumerator ResourceCollectError(string error)
    {
        progressBarStatus.text = (error);

        yield return new WaitForSeconds(2);

        progressBarStatus.text = ""; 
    }

    IEnumerator GetResourceTimer()
    {
        timeToWait = 5;

        timerRunning = true;

        progressBarStatus.text = "";
        getResource = true; // getresource is the issue
        
        yield return new WaitForSeconds(timeToWait);

        

        timerRunning = false;

    }


    IEnumerator GetResource()
    {

        gettingResource = true;

        if (getResource)
        {
            //items.Add((itemType)item[0]);

            
            resourceAmountToAdd = (int)item[2];

            //print(tmp.name);
            Debug.Log((int)item[1]);

            for (int i = 0; i < nearbyResources.Count; i++)
            {
                if (nearbyResources[i] != null && nearbyResources.Count < 2)
                {
                    items.Add((itemType)nearbyResources[i].currentType);
                    if (items[0] == itemType.wood)
                    {
                        woodAmount += resourceAmountToAdd;

                        if ((int)item[1] == nearbyResources[i].currentID)
                            Destroy(nearbyResources[i].gameObject);

                        nearbyResources = null;
                        items.Clear();

                    }
                    else if (items[0] == itemType.stone)
                    {
                        stoneAmount += resourceAmountToAdd;

                        if ((int)item[1] == nearbyResources[i].currentID)
                            Destroy(nearbyResources[i].gameObject);
                        nearbyResources = null;
                        items.Clear();
                    }
                    else if (items[0] == itemType.food)
                    {
                        foodAmount += resourceAmountToAdd;

                        if ((int)item[1] == nearbyResources[i].currentID)
                            Destroy(nearbyResources[i].gameObject);
                        nearbyResources = null;
                        items.Clear();
                    }
                }
                else { print("Standing near too many objects."); }
            }



            yield return new WaitForSeconds(1);
            canGetResource = false;
            getResource = false;
            tmp = null;
        }
        gettingResource = false;
        
    }

    #endregion
    
    //void OnTriggerEnter(Collider other)
    //{

    //    //testForMultiple
    //    if (other.tag == "resource")
    //    {
    //        resourceHits.Add(other.gameObject);

    //        if (resourceHits.Count > 1)
    //        {
    //           // print("Multiple Resources Are Closeby.");
    //        }
    //    }
    //}
    //void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "resource")
    //    {
    //        resourceHits.Remove(other.gameObject);
    //    }
    //}

}
