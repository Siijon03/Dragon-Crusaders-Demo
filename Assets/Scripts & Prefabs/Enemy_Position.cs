using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Position : MonoBehaviour
{
    //Public Access
    public float x = 0f;
    public float y = 0f;

  

    //This stores the coordinates 
    //By making this public, we can send it to start and then pass it to public.
    public Vector3 Start_Position = new Vector3(0, 0, 0);

    //This stores the player's co-ordinates 
    public Vector2 Enemy_Placement = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            //This accesses our grid manager script and Creates a new variable 
            GameObject GridObject = GameObject.Find("Enemy_GridManager");
            //This accesses our grid manager script and calls upon those functions
            Enemy_GridManager GridScript = GridObject.GetComponent<Enemy_GridManager>();
            //Gets the values of the rows and columns 
            y = GridScript.E_Grid_Row;
            x = GridScript.E_Grid_Columns;

            //Rounds off the numbers 
            //We need 'new' values so it doesn't conflict with the old ones.
            //Additionally we need to find half the positions while rounding it so we don't overlap positions and can spawn perfectly in the middle 
            float new_y = (float)(Math.Ceiling((y / 2)));
            float new_x = ((float)(Math.Ceiling(x / 2)));

            //This makes it so the player will spawn in the middle by adjusting those co-ordinates 
            gameObject.transform.position = new Vector2(GridObject.transform.position.x + (new_x - 1), GridObject.transform.position.y + ((new_y * -1) + 1));
            //Store enemy position as local co-ordinates
            Enemy_Placement = new Vector2(new_x, new_y);
            //This stores the starting position. 
            Start_Position = gameObject.transform.position;
        }
        //Just in case the code cannot find the grid 
        catch (Exception e)
        {
            Debug.Log("Enemy did not Spawn :(");
        }
    }

    // This is so the player can move around easily
    //This transforms the position of the game objects
    void Update()
    {

    }

}
        
    
