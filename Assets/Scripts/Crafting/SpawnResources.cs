using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/* This script was developed by Drennen Dooms. All rights reserved. */

public class SpawnResources : MonoBehaviour {

    // Resource SpawnPoint Prefabs
    public GameObject stoneSpawn, woodSpawn, foodSpawn;

    // Spawn rate is based on rarity
    public int stoneRarity, woodRarity, foodRarity;

    //Resource Prefabs
    public GameObject[] stones, woods, foods;

    // Range of Spawn Area
    public int stoneRangeMin = -5, stoneRangeMax = 5, 
                woodRangeMin = -5, woodRangeMax = 5, 
                foodRangeMin = -5, foodRangeMax = 5;
    
    void Start()
    {

        #region SPAWN ALL RESOURCES
        for (int s = 0; s < stones.Length; s++)
        {
            for (int _s = 0; _s < stoneRarity * 10; _s++)
            {
                GameObject newStone = Instantiate(stones[s]);
                newStone.transform.position = new Vector3(stoneSpawn.transform.position.x - (Random.Range(stoneRangeMin,stoneRangeMax)), stoneSpawn.transform.position.y, stoneSpawn.transform.position.z - (Random.Range(stoneRangeMin, stoneRangeMax)));
                newStone.transform.localScale = new Vector3(5f, 5f, 5f);
            }
            
        }

        for (int w = 0; w < woods.Length; w++)
        {
            for (int _w = 0; _w < woodRarity * 10; _w++)
            {
                GameObject newWood = Instantiate(woods[w]);

                newWood.transform.position = new Vector3(woodSpawn.transform.position.x - (Random.Range(woodRangeMin, woodRangeMax)), woodSpawn.transform.position.y, stoneSpawn.transform.position.x - (Random.Range(woodRangeMin, woodRangeMax)));
                newWood.transform.localScale = new Vector3(10f, 10f, 10f);
            }
        }

        for (int f = 0; f < foods.Length; f++)
        {
            for (int _f = 0; _f < foodRarity * 10; _f++)
            {
                GameObject newFood = Instantiate(foods[f]);
                newFood.transform.position = new Vector3(foodSpawn.transform.position.x - (Random.Range(foodRangeMin, foodRangeMax)), foodSpawn.transform.position.y, foodSpawn.transform.position.x - (Random.Range(foodRangeMin, foodRangeMax)));
                newFood.transform.localScale = new Vector3(5f, 5f, 5f);
            }
        }

        #endregion
        
    }

    


}
