using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    public class ObjectPools
    {
        private GameContext gameContext;
        private ObjectPool<GameObject> snakePool;
        private ObjectPool<GameObject> obstaclePool;
        private ObjectPool<GameObject> fruitPool;
        
        private GameObject snakePrefab;
        private GameObject obstaclePrefab;
        private GameObject fruitPrefab;
        
        public ObjectPool<GameObject> SnakePool => snakePool;

        public ObjectPool<GameObject> ObstaclePool => obstaclePool;

        public ObjectPool<GameObject> FruitPool => fruitPool;

        public ObjectPools(GameContext gameContext, GameObject snakePrefab, GameObject obstaclePrefab, GameObject fruitPrefab)
        {
            this.gameContext = gameContext;
            this.snakePrefab = snakePrefab;
            this.obstaclePrefab = obstaclePrefab;
            this.fruitPrefab = fruitPrefab;
            
            PopulatePools();
        }
        
        private void PopulatePools()
        {
            snakePool = new ObjectPool<GameObject>(() => { return Object.Instantiate(snakePrefab); },
                snakeSegment => { snakeSegment.gameObject.SetActive(true); },
                snakeSegment => { snakeSegment.gameObject.SetActive(false); },
                snakeSegment => { Object.Destroy(snakeSegment.gameObject); }, false, 10, 10);

            obstaclePool = new ObjectPool<GameObject>(() => { return Object.Instantiate(obstaclePrefab); },
                obstacle => { obstacle.gameObject.SetActive(true); },
                obstacle => { obstacle.gameObject.SetActive(false); },
                obstacle => { Object.Destroy(obstacle.gameObject); }, false, 5, 5);

            fruitPool = new ObjectPool<GameObject>(() => { return Object.Instantiate(fruitPrefab); },
                fruit => { fruit.gameObject.SetActive(true); },
                fruit => { fruit.gameObject.SetActive(false); },
                fruit => { Object.Destroy(fruit.gameObject); }, false, 1, 1);
        }
        
        public void ReleasePoolObjects()
        {
            foreach (var snakeSegment in gameContext.Snake.Segments)
            {
                snakePool.Release(snakeSegment.TileObject);
            }
            
            foreach (var obstacle in gameContext.Field.Obstacles)
            {
                obstaclePool.Release(obstacle);
            }

            foreach (var fruit in gameContext.Field.Fruits)
            {
                fruitPool.Release(fruit);
            }
        }
    }
}