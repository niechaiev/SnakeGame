using System;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class GameContext : MonoBehaviour
    {
        
        [SerializeField] private GameObject emptyTile;
        [SerializeField] private GameObject snakeTile;
        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject tilesParent;
        
        private int fieldWidth = 20;
        private int fieldHeight = 20;
        private float spacing = 1.1f;
        private int snakeStartingSize = 2;

        private readonly float cameraOffset = -0.5f;
        private Snake snake;
        private Field field;
        private Game game;

        private void Start()
        {
            RecenterCamera();
            var initPos = new Vector2Int(fieldWidth / 2, fieldHeight / 2); 
            snake = new Snake(initPos, snakeStartingSize); 
            field = new Field(fieldWidth, fieldHeight); 
            game = new Game(snake, field); 
            DrawField(field.Tiles);
        }
        
        private void InstantiateTile(int i, int j, GameObject prefab)
        {
            var newTile = Instantiate(prefab, new Vector3(i * spacing, 0, j * spacing),
                prefab.transform.rotation);
            newTile.transform.parent = tilesParent.transform;
        }

        private void RecenterCamera()
        {
            var offsetSpacingMultiply = cameraOffset * spacing;
            
            var cameraTransform = new Vector3(
                offsetSpacingMultiply + fieldWidth / 2f * spacing, 
                fieldWidth * 2 * spacing,
                offsetSpacingMultiply + fieldHeight / 2f * spacing);
            
            gameCamera.transform.position = cameraTransform;
        }
        
        private void DrawField(Tile[,] tiles)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    switch (tiles[i,j].TileType)
                    {
                        case TileType.Empty:
                            InstantiateTile(i,j,emptyTile);
                            break;
                    }
                }
            }
        }
    }
}