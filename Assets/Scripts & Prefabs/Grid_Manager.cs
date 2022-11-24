using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Manager : MonoBehaviour
{
    //Serialization is the automatic process of transforming data structures
    //or object states into a format that Unity can store and reconstruct later.
    [SerializeField] private int Grid_Row = 3;

    [SerializeField] private int Grid_Columns = 3;

    [SerializeField] private int Grid_Tile_Size = 1;

    [SerializeField] private Transform _camera;

    public Tile _tilePrefab;

    private Dictionary<Vector2, Tile> _tiles;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        GameObject referenceTile = (GameObject)Instantiate(Resources.Load("Game Tile"));

        //Sets the Width of the Grid
        for (int x = 0; x < Grid_Row; x++)
        {
            //Sets the Height of the Grid
            for (int y = 0; y < Grid_Columns; y++)
            {
                GameObject tile = (GameObject)Instantiate(referenceTile, transform);

                float posX = Grid_Columns * Grid_Tile_Size;
                float posY = Grid_Row * -Grid_Tile_Size;

                tile.transform.position = new Vector2(posX, posY);
            }
        }

        Destroy(referenceTile);

        //_camera.transform.position = new Vector3((float)_width/2 -0.5f - 1 ,(float)_height / 2 - 3.5f, - 10);

        Destroy(gameObject);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }
}
https://www.youtube.com/watch?v=u2_O-jQDD6s