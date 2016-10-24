using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/* This script was developed by Drennen Dooms. All rights reserved. */

public class Commands : MonoBehaviour {

    public Text cmdTxt;
    string[] cmd;
    bool showCmd;

    void Update()
    {
        if (Input.GetButtonDown("Cmd") && showCmd == false)
        {
            GetComponent<PlayerController>().canMove = false;
            GetComponent<PlayerController>().turnSpeedModifier = 0f;
            showCmd = true;
        }
        else if(Input.GetButtonDown("Cmd") && showCmd == true)
        {
            GetComponent<PlayerController>().canMove = true;
            GetComponent<PlayerController>().turnSpeedModifier = GetComponent<PlayerController>().defaultTurnSpeedMod;
            showCmd = false;
        }
    }

    public void Submitted()
    {

        

        if (cmdTxt.text.Contains("spawn"))
        {
            cmd = cmdTxt.text.Split(' ');
            if (cmd[1].Contains("wood"))
            {
                int o;
                if(int.TryParse(cmd[2], out o) == false)
                {
                    Debug.Log("Invalid Amount. Only use integers.");
                }

                Spawn(cmd[1].ToString(), o);
            }
        }

    }

    void Spawn(string obj, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject spawned = Instantiate(GameObject.Find(obj));
            spawned.name = "Spawned_OBJ_" + obj + " "+ "(" + i + ")";
            spawned.transform.position = transform.position + new Vector3(1+ i, 1+ i, 1+ i);
            spawned.transform.localScale = GameObject.Find(obj).transform.localScale * 10;
        }
    }


}
