using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Game
{
    public class GameContext : MonoBehaviour
    {
        [SerializeField] private GameObject emptyTilePrefab;
        [SerializeField] private GameObject snakePrefab;
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private GameObject fruitPrefab;
        
        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject emptyTilesParent;
        [SerializeField] private PlayerControlsUI playerControlsUI;

        private readonly int fieldWidth = 20;
        private readonly int fieldHeight = 20;
        private readonly float spacing = 1.1f;
        private readonly int snakeStartingSize = 2;
        private readonly float snakeSpeed = 0.5f;
        private readonly float cameraOffset = -0.5f;
        
        private ObjectPools objectPools;
        private Coroutine intervalCoroutine;
        
        private Snake snake;
        private Field field;
        private Game game;

        public Snake Snake => snake;
        public Field Field => field;
        public Game Game => game;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            objectPools = new ObjectPools(this, snakePrefab, obstaclePrefab, fruitPrefab);
            RecenterCamera();
            InitializeGame();
            InstantiateEmptyTiles(field.Tiles);
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

        private void InitializeGame()
        {
            field = new Field(fieldWidth, fieldHeight,
                3,5,1,
                AddFruitReferences);
            
            InitializeSnake();
            game = new Game(snake, field);
            intervalCoroutine = StartCoroutine(Interval());
            playerControlsUI.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            playerControlsUI.gameObject.SetActive(false);
            StopCoroutine(intervalCoroutine);
            field.UnSubscribe(AddFruitReferences);
            snake.UnSubscribe(SwapTiles, GrowSnake);
            objectPools.ReleasePoolObjects();
            InitializeGame();
        }
        
        private void AddFruitReferences(Tile fruitTile)
        {
            fruitTile.TileObject = DrawObjectFromPool(fruitTile, objectPools.FruitPool);
            field.Fruits.Add(fruitTile.TileObject);
        }
        
        private void InitializeSnake()
        {
            var snakePosition = field.Tiles[fieldWidth / 2, fieldHeight / 2];
            snake = new Snake(snakePosition, field, SwapTiles, GrowSnake);
            snake.AddSegment(field.Tiles[fieldWidth / 2, fieldHeight / 2 - 1]);
        }
        
        private void SwapTiles(Tile from, Tile to)
        {
            to.TileObject = from.TileObject;
            from.TileObject = null;
            to.TileObject.transform.position = GetObjectPosition(to);
        }
        private void GrowSnake(Tile snakeTile)
        {
            field.Fruits.Remove(snakeTile.TileObject);
            objectPools.FruitPool.Release(snakeTile.TileObject);
            snakeTile.TileObject = DrawObjectFromPool(snakeTile, objectPools.SnakePool);
        }
        IEnumerator Interval()
        {
            DrawTiles(field.Tiles);
            while (true)
            {
                yield return new WaitForSeconds(snakeSpeed);
                game.Update();
            }
        }

        private Vector3 GetObjectPosition(Tile tile)
        {
            return new Vector3(tile.Position.x * spacing, 0, tile.Position.y * spacing);
        }
        
        private void InstantiateEmptyTiles(Tile[,] tiles)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    var tileObject = Instantiate(emptyTilePrefab, GetObjectPosition(tiles[i, j]),
                        emptyTilePrefab.transform.rotation);
                    tileObject.transform.parent = emptyTilesParent.transform;
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
                            currentTile.TileObject = DrawObjectFromPool(currentTile, objectPools.SnakePool);
                            break;
                        case TileType.Fruit:
                            currentTile.TileObject = DrawObjectFromPool(currentTile, objectPools.FruitPool);
                            field.Fruits.Add(currentTile.TileObject);
                            break;
                        case TileType.Obstacle:
                            currentTile.TileObject = DrawObjectFromPool(currentTile, objectPools.ObstaclePool);
                            field.Obstacles.Add(currentTile.TileObject);
                            break;
                    }
                }
            }
        }
        
        private GameObject DrawObjectFromPool(Tile tile, ObjectPool<GameObject> pool) 
        {
            var pooledObject = pool.Get();
            pooledObject.transform.position = GetObjectPosition(tile);
            return pooledObject;
        }
        
    }
}