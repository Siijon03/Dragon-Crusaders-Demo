using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //Public Access
    public float x = 0f;
    public float y = 0f;

    //This stores the coordinates 
    //By making this public, we can send it to start and then pass it to public.
    public Vector3 Start_Position = new Vector3(0,0,0);

    //This stores the player's co-ordinates 
    public Vector2 Player_Position = new Vector2(0,0);

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            //This accesses our grid manager script and Creates a new variable 
            GameObject GridObject = GameObject.Find("GridManager");
            //This accesses our grid manager script and calls upon those functions
            Grid_Manager GridScript = GridObject.GetComponent<Grid_Manager>();
            //Gets the values of the rows and columns 
             y = GridScript.Grid_Row;
             x = GridScript.Grid_Columns;

            //Rounds off the numbers 
            //We need 'new' values so it doesn't conflict with the old ones.
            //Additionally we need to find half the positions while rounding it so we don't overlap positions and can spawn perfectly in the middle 
            float new_y = (float)(Math.Ceiling((y / 2)));
            float new_x = ((float)(Math.Ceiling(x / 2)));

            //This makes it so the player will spawn in the middle by adjusting those co-ordinates 
            gameObject.transform.position = new Vector2(GridObject.transform.position.x + (new_x - 1) , GridObject.transform.position.y + ((new_y * - 1) + 1));
            //Store player position as local co-ordinates
            Player_Position = new Vector2(new_x, new_y);
            //This stores the starting position. 
            Start_Position = gameObject.transform.position;
        }
        //Just in case the code cannot find the grid 
        catch (Exception e)
        {
            Debug.Log("Couldn't Find it :p");
        }
    }

    // This is so the player can move around easily
    //This transforms the position of the game objects
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //This is so the player can move around without going too far up.
            if (!(Player_Position.y == 1))
            {
                gameObject.transform.position += new Vector3(0f, +1f);
                Player_Position += new Vector2(0f, -1f);
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //This is so the player can move around without going too far down.
            if (!(Player_Position.y == y))
            {
                gameObject.transform.position += new Vector3(0f, -1f);
                Player_Position += new Vector2(0f, +1f);
            }
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //This is so the player can move around without going too far right.
            if (!(Player_Position.x == x))
            {
                gameObject.transform.position += new Vector3(+1f, 0f);
                Player_Position += new Vector2(+1f, 0f);
            }
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //This is so the player can move around without going too far left.
            if (!(Player_Position.x == 1))
            {
                gameObject.transform.position += new Vector3(-1f, 0f);
                Player_Position += new Vector2(-1f, 0f);
            }
        }
    }
}
