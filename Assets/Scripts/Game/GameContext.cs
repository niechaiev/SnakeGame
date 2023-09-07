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
        [SerializeField] private GameObject tilesParent;

        [SerializeField] private PlayerControlsUI playerControlsUI;

        private readonly int fieldWidth = 20;
        private readonly int fieldHeight = 20;
        private readonly float spacing = 1.1f;
        private readonly int snakeStartingSize = 2;
        private readonly float snakeSpeed = 0.5f;

        private readonly float cameraOffset = -0.5f;
        private Snake snake;
        private Field field;
        private Game game;

        private ObjectPool<GameObject> snakePool;
        private ObjectPool<GameObject> obstaclePool;
        private ObjectPool<GameObject> fruitPool;


        private Coroutine intervalCoroutine;

        public Game Game
        {
            get => game;
            set => game = value;
        }

        private void Awake()
        {
            Application.targetFrameRate = 60;
            PopulatePools();
            RecenterCamera();
            InitializeGame();
        }

        private void PopulatePools()
        {
            snakePool = new ObjectPool<GameObject>(() => { return Instantiate(snakePrefab); },
                snakeSegment => { snakeSegment.gameObject.SetActive(true); },
                snakeSegment => { snakeSegment.gameObject.SetActive(false); },
                snakeSegment => { Destroy(snakeSegment.gameObject); }, false, 10, 10);

            obstaclePool = new ObjectPool<GameObject>(() => { return Instantiate(obstaclePrefab); },
                obstacle => { obstacle.gameObject.SetActive(true); },
                obstacle => { obstacle.gameObject.SetActive(false); },
                obstacle => { Destroy(obstacle.gameObject); }, false, 5, 5);

            fruitPool = new ObjectPool<GameObject>(() => { return Instantiate(fruitPrefab); },
                fruit => { fruit.gameObject.SetActive(true); },
                fruit => { fruit.gameObject.SetActive(false); },
                fruit => { Destroy(fruit.gameObject); }, false, 1, 1);
        }

        private void Start()
        {
            DrawEmptyTiles(field.Tiles);
        }

        public void InitializeGame()
        {
            field = new Field(fieldWidth, fieldHeight,3,5,1);
            InitializeSnake();
            game = new Game(snake, field);
            intervalCoroutine = StartCoroutine(Interval());
            playerControlsUI.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            playerControlsUI.gameObject.SetActive(false);
            StopCoroutine(intervalCoroutine);
            snake.OnMove -= SwapTiles;
            snake.OnGrow -= PoolSnakeSegment;
            ReleasePooledObjects();
            InitializeGame();
        }

        private void ReleasePooledObjects()
        {
            foreach (var snakeSegment in snake.Segments)
            {
                snakePool.Release(snakeSegment.TileObject);
            }
            
            foreach (var obstacle in field.Obstacles)
            {
                obstaclePool.Release(obstacle);
            }

            foreach (var fruit in field.Fruits)
            {
                fruitPool.Release(fruit);
            }
        }

        private void InitializeSnake()
        {
            var snakePosition = field.Tiles[fieldWidth / 2, fieldHeight / 2];
            snake = new Snake(snakePosition, field);
            snake.AddSegment(field.Tiles[fieldWidth / 2, fieldHeight / 2 - 1]);

            snake.OnMove += SwapTiles;
            snake.OnGrow += PoolSnakeSegment;
        }

        private void PoolSnakeSegment(Tile snakeTile)
        {
            fruitPool.Release(snakeTile.TileObject);
            snakeTile.TileObject = PoolObject(snakeTile, snakePool);
        }

        private void SwapTiles(Tile from, Tile to)
        {
            to.TileObject = from.TileObject;
            from.TileObject = null;
            to.TileObject.transform.position = GetObjectPosition(to);
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
                            currentTile.TileObject = PoolObject(currentTile, snakePool);
                            break;
                        case TileType.Fruit:
                            currentTile.TileObject = PoolObject(currentTile, fruitPool);
                            field.Fruits.Add(currentTile.TileObject);
                            break;
                        case TileType.Obstacle:
                            currentTile.TileObject = PoolObject(currentTile, obstaclePool);
                            field.Obstacles.Add(currentTile.TileObject);
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

        private GameObject PoolObject(Tile tile, ObjectPool<GameObject> pool) 
        {
            var pooledObject = pool.Get();
            pooledObject.transform.position = GetObjectPosition(tile);
            return pooledObject;
        }
        
    }
}