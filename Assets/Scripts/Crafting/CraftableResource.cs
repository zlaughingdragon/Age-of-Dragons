using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;



/* This script was developed by Drennen Dooms. All rights reserved. 

    test change

*/

public class CraftableResource : MonoBehaviour {

    public int resourceAmount, resourceMax = 50;
    
    public Transform player;
    private Image keyImg; // keyboard key image
    public bool transition;

    
    
    //Static is the real value. Else it is meant for the inspector.

    public enum itemType
    {
        wood, stone, food
    }

    static itemType type;

    public itemType currentType;

    static List<itemType> items = new List<itemType>();
    
    public static int ID = 0, itemCount;
    public int currentID, currentItemCount;

    

    //sending the CURRENT TYPE SET BY DEV AND BASED ON RESOURCE IN GAME WORLD through ArrayList item.
    //sending the CURRENT ID for reference.
    //sending the ITEM COUNT randomly set at start.
    ArrayList item;

    Color clr;
    
    void Awake()
    {
        
    }

    void Start()
    {
        
        
        keyImg = transform.FindChild("Canvas/interactImg").GetComponent<Image>();
        //keyImg.enabled = false;
        clr = keyImg.color;
        clr.a = 0;

        resourceAmount = Random.Range(1, 5);



        //currentType = itemType.wood;

        //testing type
        //if (items[0] == itemType.wood)
        //{
        //    print("boom it's wood.");
        //}
    }

    void Update()
    {
        
        if (transition) {
            
                    clr.a += Time.deltaTime * 1.8f;
            keyImg.color = clr;
            if (clr.a > 1f)
                    {
                        clr.a = 1f;
                    }

        }else
        {
            
            clr.a += Time.deltaTime * -1.8f;
            keyImg.color = clr;
            if (clr.a < 0f)
            {
                clr.a = 0f;
            }

        }

        

    }

    void OnTriggerEnter(Collider other)
    {
        ID = Random.Range(1000000, 9999999);
        player = GameObject.FindGameObjectWithTag("Player").transform.root;

        #region trigger
        if (other.tag == "Player") // NOTE: ADD MAX RESOURCE MECHANIC
        {
            resourceAmount = Random.Range(1, 5);
            type = currentType;
            itemCount = resourceAmount;
            currentID = ID;
            items.Add(type);
            
            item = new ArrayList { type, ID, itemCount };

            clr.a = 0f;
            transition = true;

            //Player is near a resource and can get it.
            player.GetComponent<Craftables>().canGetResource = true;


            //currently deprecated
            player.GetComponent<Craftables>().tmp = gameObject;
            

            player.GetComponent<Craftables>().item = item;
            //print("Type: " + (itemType)item[0]);
            //print("ID: " + (int)item[1]);
            //print("Amount: " + (int)item[2]);

            //player.GetComponent<Craftables>().nearbyResources.Add(gameObject);
        }
        #endregion


        //else if(player.GetComponent<Craftables>().resourceAmountToAdd > resourceMax)
        //{
        //    Debug.Log("Too many resources.. cannot collect.");
        //}

    }
    void OnTriggerExit(Collider other)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.root;

        if (other.tag == "Player")
        {
            clr.a = 1f;
            transition = false;
            //keyImg.enabled = false;
            player.GetComponent<Craftables>().canGetResource = false;
            player.GetComponent<Craftables>().tmp = null;
            player.GetComponent<Craftables>().item = null;
        }
    }



}
