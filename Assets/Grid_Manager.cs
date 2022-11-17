using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Manager : MonoBehaviour
{
    //Serialization is the automatic process of transforming data structures
    //or object states into a format that Unity can store and reconstruct later.
    [SerializeField] private int _width, _height;

    public Tile _tilePrefab;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        //Sets the Width of the Grid
        for (int x = 0; x < 3; x++)
        {
            //Sets the Height of the Grid
            for (int y = 0; y < 3; y++)
            {
                //Instantiate that allows you to spawn new objects in the scene. Vector 3 represents 3D vectors and points. 
                //"Quaternion. identity" is the "default" or none value to the objects rotation. By setting the rotation of the new object to this value it ensures that the new object will be in it's "natural" orientation.
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile{x}{y}";
            }
        }

        Destroy(gameObject);
    }
}
