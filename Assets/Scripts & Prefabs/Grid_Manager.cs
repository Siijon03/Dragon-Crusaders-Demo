using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Manager : MonoBehaviour
{
    //Serialization is the automatic process of transforming data structures
    //or object states into a format that Unity can store and reconstruct later.
    //Making these public mean we can create a 'dynamic' grid 
    [SerializeField] public int Grid_Row = 3;

    [SerializeField] public int Grid_Columns = 3;

    [SerializeField] public int Grid_Tile_Size = 1;

    [SerializeField] private Transform _camera;

    public GameObject _tilePrefab;

    

    private Dictionary<Vector2, Tile> _tiles;


    //Starts by making the grid
    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        //GameObject referenceTile = (GameObject)Instantiate(Resources.Load("Game Tile"));

        //Sets the Width of the Grid
        for (int x = 0; x < Grid_Row; x++)
        {
            //Sets the Height of the Grid
            for (int y = 0; y < Grid_Columns; y++)
            {
                //This Generates and calls the tile prefab so it can be transformed
                GameObject tile = (GameObject)Instantiate(_tilePrefab, transform);
                //Checking if tiles were actually made
                Debug.Log("Tile Made");

                //Gets the positions for the tile sizes
                float posX = y * Grid_Tile_Size;
                float posY = x * -Grid_Tile_Size;

                //Moves the tiles into those spaces
                tile.transform.position = new Vector2(posX, posY);
            }
        }

       

       
        

        //Instamtiate player to move a fixed number so it looks like they're in the tiles 
        //Enemy attacks space of tile 
    }
}
//https://www.youtube.com/watch?v=u2_O-jQDD6s