using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GridManager : MonoBehaviour
{
    //Serialization is the automatic process of transforming data structures
    //or object states into a format that Unity can store and reconstruct later.
    //Making these public mean we can create a 'dynamic' grid 
    [SerializeField] public int Enemy_Grid_Row = 1;

    [SerializeField] public int Enemy_Grid_Columns = 1;

    [SerializeField] public int Enemy_Grid_Tile_Size = 1;

    [SerializeField] private Transform _camera;

    public GameObject _EnemytilePrefab;

    //This Will make the EnemyGrid
    private void Start()
    {
        GenerateEnemyGrid();
    }

    //Making the Enemy's Grid
    void GenerateEnemyGrid()
    {
        //Sets the Width of the Grid
        for (int x = 0; x < Enemy_Grid_Row; x++)
        {
            //Sets the Height of the Grid
            for (int y = 0; y < Enemy_Grid_Columns; y++)
            {
                //This Generates and calls the tile prefab so it can be transformed
                GameObject EnemyTile = (GameObject)Instantiate(_EnemytilePrefab, transform);
                //Checking if tiles were actually made
                Debug.Log("Tile Made");

                //Gets the positions for the tile sizes
                float posX = y * Enemy_Grid_Tile_Size;
                float posY = x * -Enemy_Grid_Tile_Size;

                //Moves the tiles into those spaces
                EnemyTile.transform.position = new Vector2(gameObject.transform.position.x + posX, gameObject.transform.position.y + posY);
            }
        }

        float GridW = Enemy_Grid_Columns * Enemy_Grid_Tile_Size;
        float GridH = Enemy_Grid_Row * Enemy_Grid_Tile_Size;

        //Instamtiate player to move a fixed number so it looks like they're in the tiles 
        //Enemy attacks space of tile 
    }
}
