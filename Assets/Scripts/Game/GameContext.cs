using System.Collections;
using UnityEngine;

namespace Game
{
    public class GameContext : MonoBehaviour
    {
        [SerializeField] private GameObject emptyTilePrefab;
        [SerializeField] private GameObject snakeTilePrefab;
        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject tilesParent;

        private readonly int fieldWidth = 20;
        private readonly int fieldHeight = 20;
        private readonly float spacing = 1.1f;
        private readonly int snakeStartingSize = 2;
        private readonly float snakeSpeed = 0.5f;

        private readonly float cameraOffset = -0.5f;
        private Snake snake;
        private Field field;
        private Game game;

        private void Start()
        {
            Application.targetFrameRate = 60;

            RecenterCamera();
            field = new Field(fieldWidth, fieldHeight);

            var snakePosition = field.Tiles[fieldWidth / 2, fieldHeight / 2];
            snake = new Snake(snakePosition, snakeStartingSize, field);
            snake.Swap += SwapTiles;
            game = new Game(snake, field);
            DrawEmptyTiles(field.Tiles);
            StartCoroutine(Interval());
        }

        private void SwapTiles(Tile from, Tile to)
        {
            to.TileObject = from.TileObject;
            from.TileObject = null;
            to.TileObject.transform.position = GetObjectPosition(to);
        }

        private void Update()
        {
        }

        IEnumerator Interval()
        {
            DrawTiles(field.Tiles);
            while (true)
            {
                yield return new WaitForSeconds(snakeSpeed);
                game.Update();
                DrawTiles(field.Tiles);
            }
        }


        private Vector3 GetObjectPosition(Tile tile)
        {
            return new Vector3(tile.Position.x * spacing, 0, tile.Position.y * spacing);
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

        private void DrawEmptyTiles(Tile[,] tiles)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    InstantiateTileObject(tiles[i, j], emptyTilePrefab);
                }
            }
        }

        private void DrawTiles(Tile[,] tiles)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    var currentTile = tiles[i, j];
                    
                    if (currentTile.TileObject != null)
                        continue;
                    
                    switch (currentTile.TileType)
                    {
                        case TileType.Snake: 
                            currentTile.TileObject = InstantiateTileObject(currentTile, snakeTilePrefab);
                            break;
                        case TileType.Fruit:
                            break;
                        case TileType.Obstacle:
                            break;
                    }
                }
            }
        }
        
        private GameObject InstantiateTileObject(Tile tile, GameObject prefab)
        {
            var tileObject = Instantiate(prefab, GetObjectPosition(tile),
                prefab.transform.rotation);
            tileObject.transform.parent = tilesParent.transform;
            return tileObject;
        }
    }
}