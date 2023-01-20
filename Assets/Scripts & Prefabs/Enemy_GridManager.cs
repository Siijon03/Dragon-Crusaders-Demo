using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Grid_Manager : MonoBehaviour
{
    //Serialization is the automatic process of transforming data structures
    //or object states into a format that Unity can store and reconstruct later.
    //Making these public mean we can create a 'dynamic' grid 
    [SerializeField] public int E_Grid_Row = 1;

    [SerializeField] public int E_Grid_Columns = 1;

    [SerializeField] public int E_Grid_Tile_Size = 1;

    [SerializeField] private Transform _camera;

    public GameObject _EtilePrefab;



    private Dictionary<Vector2, Tile> _tiles;


    //Starts by making the grid
    private void Start()
    {
        E_GenerateGrid();
    }

    void E_GenerateGrid()
    {
        //GameObject referenceTile = (GameObject)Instantiate(Resources.Load("Game Tile"));

        //Sets the Width of the Grid
        for (int x = 0; x < E_Grid_Row; x++)
        {
            //Sets the Height of the Grid
            for (int y = 0; y < E_Grid_Columns; y++)
            {
                //This Generates and calls the tile prefab so it can be transformed
                GameObject tile = (GameObject)Instantiate(_EtilePrefab, transform);
                //Checking if tiles were actually made
                Debug.Log("Enemy Tile Made");

                //Gets the positions for the tile sizes
                float posX = y * E_Grid_Tile_Size;
                float posY = x * -E_Grid_Tile_Size;

                //Moves the tiles into those spaces
                tile.transform.position = new Vector2(gameObject.transform.position.x + posX, gameObject.transform.position.y + posY);
            }
        }

        float GridW = E_Grid_Columns * E_Grid_Tile_Size;
        float GridH = E_Grid_Row * E_Grid_Tile_Size;
        //transform.position = new Vector2((-GridW / 2) - 8 + Grid_Tile_Size / 2, (GridH / 2) - 1 - Grid_Tile_Size / 2);




        //Instamtiate player to move a fixed number so it looks like they're in the tiles 
        //Enemy attacks space of tile 
    }
}
//https://www.youtube.com/watch?v=u2_O-jQDD6s