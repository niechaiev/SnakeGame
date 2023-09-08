using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    public class GameVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject emptyTilePrefab;
        [SerializeField] private GameObject snakePrefab;
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private GameObject fruitPrefab;

        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject emptyTilesParent;
        [SerializeField] private PlayerControlsUI playerControlsUI;

        [SerializeField] private EndScreenUI endScreenUI;
        [SerializeField] private CounterScreenUI counterScreenUI;

        [SerializeField] private AudioClip fruitSound;
        [SerializeField] private AudioClip winSound;
        [SerializeField] private AudioClip loseSound;
        [SerializeField] private AudioSource audioSource;
        
        private readonly float spacing = 1.1f;
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

            InitializeGame();
            RecenterCamera();
            InstantiateEmptyTiles(field.Tiles);
        }

        private void RecenterCamera()
        {
            var offsetSpacingMultiply = cameraOffset * spacing;

            var cameraTransform = new Vector3(
                offsetSpacingMultiply + field.Width / 2f * spacing,
                field.Width * 2 * spacing,
                offsetSpacingMultiply + field.Height / 2f * spacing);

            gameCamera.transform.position = cameraTransform;
        }

        private void InitializeGame()
        {
            field = new Field(AddFruitReferences);
            InitializeSnake();
            game = new Game(snake, field, EndGame);
            DrawTiles(field.Tiles);

            intervalCoroutine = StartCoroutine(Interval());
            playerControlsUI.gameObject.SetActive(true);
            endScreenUI.HideEndScreen();
        }

        public void RestartGame()
        {
            StopGame();
            objectPools.ReleasePoolObjects();
            InitializeGame();
        }

        private void StopGame()
        {
            playerControlsUI.gameObject.SetActive(false);
            StopCoroutine(intervalCoroutine);
            field.UnSubscribe(AddFruitReferences);
            snake.UnSubscribe(SwapTiles, GrowSnake);
        }

        private void AddFruitReferences(Tile fruitTile)
        {
            fruitTile.TileObject = DrawObjectFromPool(fruitTile, objectPools.FruitPool);
            field.Fruits.Add(fruitTile.TileObject);
        }

        private void InitializeSnake()
        {
            var snakePosition = field.Tiles[field.Width / 2, field.Height / 2];
            snake = new Snake(snakePosition, SwapTiles, GrowSnake, counterScreenUI.UpdateCount);
        }

        private void EndGame(bool haveWon)
        {
            if (haveWon)
            {
                endScreenUI.ShowVictoryScreen();
                audioSource.PlayOneShot(winSound);
            }
            else
            {
                endScreenUI.ShowDefeatScreen();
                audioSource.PlayOneShot(loseSound);
            }

            StopGame();
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
            snake.Speed -= snake.GrowSpeedGain;
            audioSource.PlayOneShot(fruitSound);
        }

        IEnumerator Interval()
        {
            while (true)
            {
                yield return new WaitForSeconds(snake.Speed);
                game.NextStep();
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