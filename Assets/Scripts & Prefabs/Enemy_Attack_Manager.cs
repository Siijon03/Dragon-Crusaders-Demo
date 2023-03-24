using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Attack_Manager : MonoBehaviour
{
    //Public Access
    public float x = 0f;
    public float y = 0f;

    public float new_x;
    public float new_y;

    //Creating Enemy Attacks.
    public string AttackName;
    public int AttackID;
    public float AttackDamageModifier;

    public int Grid_Tile_Size = 1;

    public enum AttackSelection {SinisterBeam, LightningBolt, Whiplash, SpikeTrap}

    [SerializeField] GameObject BeamPrefab;
    [SerializeField] GameObject LightningBoltPrefab;
    [SerializeField] GameObject WhiplashPrefab;
    [SerializeField] GameObject SpikeTrapPrefab;


    //This stores the coordinates 
    //By making this public, we can send it to start and then pass it to public.
    public Vector3 Start_Position = new Vector3(0, 0, 0);

    //This stores the player's co-ordinates 
    public Vector2 Attack_Position = new Vector2(0, 0);

    // Start is called before the first frame update
    public void Start()
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
            gameObject.transform.position = new Vector2(GridObject.transform.position.x + (new_x - 1), GridObject.transform.position.y + ((new_y * -1) + 1));
            
            //Store Attack position as local co-ordinates
            //Vector 2
            Attack_Position = new Vector2(new_x, new_y);

            //This stores the starting position. 
            //Vector 3
            Start_Position = gameObject.transform.position;
        }
        //Just in case the code cannot find the grid 
        catch (Exception e)
        {
            Debug.Log("Couldn't Find it :p");
            Debug.Log(e);

        }

        AttackSelection AttackID = (AttackSelection)Random.Range(1f,1f);
        Debug.Log(AttackID);

        try
        {
            if (AttackID == AttackSelection.SinisterBeam)
            {
                Debug.Log("Attack ID is " + AttackID);
                SinisterBeamAttack();
            }
            else if (AttackID == AttackSelection.LightningBolt)
            {
                Debug.Log("Attack ID is " + AttackID);
                LightningBoltAttack();
            }
            else if (AttackID == AttackSelection.Whiplash)
            {
                Debug.Log("Attack ID is " + AttackID);
                Whiplash();
            }
            else if (AttackID == AttackSelection.SpikeTrap)
            {
                Debug.Log("Attack ID is " + AttackID);
                SpikeTrap();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public void SinisterBeamAttack()
    {
        AttackName = "Sinister Beam";
        AttackID = 1;
        AttackDamageModifier = 1.5f;

      
        //Create An Instance of An Attack.
        GameObject BeamAttack = (GameObject)Instantiate(BeamPrefab, transform);

        float posX = y * Grid_Tile_Size;
        float posY = x * -Grid_Tile_Size;

         //Keeps and Bounds the Prefab within the Middle.
         BeamAttack.transform.position = new Vector2(BeamAttack.transform.position.x + (new_x - 1), BeamAttack.transform.position.y + ((new_y * -1) + 1));
         //Moves the Attack up.
         BeamAttack.transform.position += new Vector3(+8.5f, -1f);
         //Keeps it from going OverBounds.
         Attack_Position += new Vector2(0f, -1f);
         BeamAttack.transform.position = new Vector2(BeamAttack.transform.position.x + (posX - 3), gameObject.transform.position.y + (posY + 3));

        Destroy(gameObject, 10);
    }

    public void LightningBoltAttack()
    {

        int RandBoltX = 0;
        int RandBoltY = Random.Range(-2, 0);

        float posX = y * Grid_Tile_Size;
        float posY = x * -Grid_Tile_Size;
        //Vector2 BoltPosition;

        AttackName = "Lightning Bolt";
        AttackID = 2;
        AttackDamageModifier = 2f;

        //This accesses our grid manager script and Creates a new variable 
        GameObject LightningBoltAttack = GameObject.Find("GridManager");
        //This accesses our grid manager script and calls upon those functions
        Grid_Manager GridScript = LightningBoltAttack.GetComponent<Grid_Manager>();
        //Gets the values of the rows and columns 
        y = GridScript.Grid_Row;
        x = GridScript.Grid_Columns;

        //Rounds off the numbers 
        //We need 'new' values so it doesn't conflict with the old ones.
        //Additionally we need to find half the positions while rounding it so we don't overlap positions and can spawn perfectly in the middle 
        float new_y = (float)(Math.Ceiling((y / 2)));
        float new_x = ((float)(Math.Ceiling(x / 2)));

        //This makes it so the player will spawn in the middle by adjusting those co-ordinates 
        gameObject.transform.position = new Vector2(LightningBoltAttack.transform.position.x + (new_x - 1), LightningBoltAttack.transform.position.y + ((new_y * -1) + 1));

        //Store Attack position as local co-ordinates
        //Vector 2
        Attack_Position = new Vector2(new_x, new_y);

        //This stores the starting position. 
        //Vector 3
        Start_Position = gameObject.transform.position;

        for (int x = 0; x < 1; x++)
        {
            for (int y = 0; y < 1; y++)
            {
                GameObject LightningBolt = (GameObject)Instantiate(LightningBoltPrefab, transform);

                LightningBolt.transform.position += new Vector3(+0.54f, +2f);
                //Keeps it from going OverBounds.
                Attack_Position += new Vector2(0f, -1f);
                LightningBolt.transform.position = new Vector2(LightningBolt.transform.position.x + (RandBoltX + posX), gameObject.transform.position.y + (posY - RandBoltY + 2));
            }
        }

        Destroy(gameObject, 10);

    }

    public void Whiplash()
    {
        AttackName = "Whip Lash";
        AttackID = 3;
        AttackDamageModifier = 1.2f;

        float posX = y * Grid_Tile_Size;
        float posY = x * -Grid_Tile_Size;

        for (int y = 0; y < 3; y++) 
        {
            GameObject WhipLashAttack = (GameObject)Instantiate(WhiplashPrefab, transform);
            WhipLashAttack.transform.position = new Vector2(WhipLashAttack.transform.position.x + (new_x - 1), WhipLashAttack.transform.position.y + ((new_y * -1) + 1));

            WhipLashAttack.transform.position += new Vector3(+4.04f, 3f);
            //Keeps it from going OverBounds.
            Attack_Position += new Vector2(0f, -1f);
            WhipLashAttack.transform.position = new Vector2(WhipLashAttack.transform.position.x - posY, WhipLashAttack.transform.position.y - (posX + y - 0.510846f));

            Destroy(gameObject, 10);
        }
    }

    public void SpikeTrap()
    {
        AttackName = "Spike Trap";
        AttackID = 4;
        AttackDamageModifier = 1.4f;

        float posX = y * Grid_Tile_Size;
        float posY = x * -Grid_Tile_Size;

        for (int y = 0; y < 1; y++)
        {
            GameObject SpikeTrapAttack = (GameObject)Instantiate(SpikeTrapPrefab, transform);
            SpikeTrapAttack.transform.position = new Vector2(SpikeTrapAttack.transform.position.x + (new_x - 1), SpikeTrapAttack.transform.position.y + ((new_y * -1) + 1));

            SpikeTrapAttack.transform.position += new Vector3(+0f,0f);
            //Keeps it from going OverBounds.
            Attack_Position += new Vector2(0f, -1f);
            SpikeTrapAttack.transform.position = new Vector2(SpikeTrapAttack.transform.position.x - (posY - 3), SpikeTrapAttack.transform.position.y - (posX + y - 2));

            Destroy(gameObject,10);
        }
    }

    void Update()
    {
        
    }
}
