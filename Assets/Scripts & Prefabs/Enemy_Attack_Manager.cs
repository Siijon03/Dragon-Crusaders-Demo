using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Attack_Manager : MonoBehaviour
{
    //Public Access
    public float x = 0f;
    public float y = 0f;

    //Creating Enemy Attacks.
    public string AttackName;
    public int AttackID;
    public float AttackDamageModifier;

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
            gameObject.transform.position = new Vector2(GridObject.transform.position.x + (new_x - 1), GridObject.transform.position.y + ((new_y * -1) + 1));
            //Store player position as local co-ordinates
            Attack_Position = new Vector2(new_x, new_y);
            //This stores the starting position. 
            Start_Position = gameObject.transform.position;
        }
        //Just in case the code cannot find the grid 
        catch (Exception e)
        {
            Debug.Log("Couldn't Find it :p");
            Debug.Log(e);

        }

        AttackSelection AttackID = (AttackSelection)Random.Range(1,4);

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
    }

    public void LightningBoltAttack()
    {
        AttackName = "Lightning Bolt";
        AttackID = 2;
        AttackDamageModifier = 2f;
    }

    public void Whiplash()
    {
        AttackName = "Whip Lash";
        AttackID = 3;
        AttackDamageModifier = 1.2f;
    }

    public void SpikeTrap()
    {
        AttackName = "Spike Trap";
        AttackID = 4;
        AttackDamageModifier = 1.4f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
