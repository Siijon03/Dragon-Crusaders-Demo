using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Position : MonoBehaviour
{

    //Public Float that Can used later.
    public float x = 0f;
    public float y = 0f;

    //Using a Public Vector 3, we can store coordinates and use it in our script.
    public Vector3 Enemy_Start_Position = new Vector3(0,0,0);

    //With this Vector 2 we can store and track the Enemy's Position on their Grid.
    public Vector2 Enemy_Grid_Position = new Vector2(0, 0);

    public GameObject EnemyPrefab;

    void Start()
    {
        try
        {
            //This accesses our grid manager script and Creates a new variable 
            GameObject Enemy_GridObject = GameObject.Find("E.GridManager");
            //This accesses our grid manager script and calls upon those functions
            Enemy_GridManager EnemyGridScript = Enemy_GridObject.GetComponent<Enemy_GridManager>();
            //Gets the values of the rows and columns 
            y = EnemyGridScript.Enemy_Grid_Row;
            x = EnemyGridScript.Enemy_Grid_Columns;

            //Rounds off the numbers 
            //We need 'new' values so it doesn't conflict with the old ones.
            //Additionally we need to find half the positions while rounding it so we don't overlap positions and can spawn perfectly in the middle 
            float new_y = (float)Math.Ceiling(y / 2);
            float new_x = (float)Math.Ceiling(x / 2);

            //This makes it so the player will spawn in the middle by adjusting those co-ordinates 
            gameObject.transform.position = new Vector2(Enemy_GridObject.transform.position.x + (new_x - 1), Enemy_GridObject.transform.position.y + ((new_y * -1) + 1));
            //Store player position as local co-ordinates
            Enemy_Grid_Position = new Vector2(new_x, new_y);
            //This stores the starting position. 
            Enemy_Start_Position = gameObject.transform.position;
        }
        //Just in case the code cannot find the grid 
        catch (Exception e)
        {
            Debug.Log("Couldn't Find Enemy");
            Debug.Log(e);

        }
    }

}
        
    
