using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class Field
    {
        private readonly int width = 20;
        private readonly int height = 20;
        private readonly int minObstaclesAmount = 3;
        private readonly int maxObstaclesAmount = 5;
        private readonly int fruitAmount = 1;
        
        private readonly Random random;
        private readonly Tile[,] tiles;
        private readonly List<GameObject> obstacles;
        private readonly List<GameObject> fruits;
        public int Width => width;
        public int Height => height;
        public Tile[,] Tiles => tiles;
        public List<GameObject> Obstacles => obstacles;
        public List<GameObject> Fruits => fruits;
        
        private Action<Tile> onGenerateFruit;

        public Field(Action<Tile> onGenerateFruit)
        {
            this.onGenerateFruit += onGenerateFruit;

            obstacles = new List<GameObject>();
            fruits = new List<GameObject>();

            tiles = new Tile[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tiles[i, j] = new Tile(new Vector2Int(i, j));
                }
            }

            random = new Random();

            GenerateObstacles();
        }

        public void UnSubscribe(Action<Tile> onGenerateFruit)
        {
            this.onGenerateFruit -= onGenerateFruit;
        }

        public void GenerateFruit()
        {
            for (int i = 0; i < fruitAmount; i++)
            {
                int row = random.Next(0, height);
                int column = random.Next(0, width);
                if (tiles[column, row].TileType != TileType.Empty)
                {
                    i--;
                    continue;
                }

                tiles[column, row].TileType = TileType.Fruit;
                onGenerateFruit.Invoke(tiles[column, row]);
            }
        }

        private void GenerateObstacles()
        {
            int obstaclesAmount = random.Next(minObstaclesAmount, maxObstaclesAmount);
            for (int i = 0; i < obstaclesAmount; i++)
            {
                int row = random.Next(0, height);
                int column = random.Next(0, width);
                if (tiles[column, row].TileType != TileType.Empty)
                {
                    i--;
                    continue;
                }

                tiles[column, row].TileType = TileType.Obstacle;
            }
        }
    }
}