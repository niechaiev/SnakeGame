using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject emptyTile;
    [SerializeField] private GameObject snakeTile;
    [SerializeField] private GameObject gameCamera;
    
    
    
    [SerializeField] private int fieldWidth = 1;
    [SerializeField] private int fieldHeight = 1;
    [SerializeField] private float spacing = 1.1f;
    private Tile[,] field;


    private void Start()
    {
        field = new Tile[fieldWidth, fieldHeight];
        var cameraTransform = new Vector3(fieldWidth / 2f * spacing, 0, fieldHeight / 2f * spacing);
        gameCamera.transform.position += cameraTransform;
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                switch (field[i,j])
                {
                    case Tile.Empty:
                        Instantiate(emptyTile, new Vector3(i * spacing, 0, j * spacing), emptyTile.transform.rotation);
                        break;
                    case Tile.Snake:
                        Instantiate(snakeTile, new Vector3(i * spacing, 0, j * spacing), emptyTile.transform.rotation);
                        break;
                }
            }
        }
    }
}