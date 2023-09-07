using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class Field
    {
        private readonly int fieldWidth;
        private readonly int fieldHeight;
        private readonly int minObstaclesAmount;
        private readonly int maxObstaclesAmount;
        private int fruitAmount;
        
        private Random random;
        private Tile[,] tiles;
        private List<GameObject> obstacles;
        private List<GameObject> fruits;
        
        public Action<Tile> OnGenerateFruit;

        public List<GameObject> Obstacles
        {
            get => obstacles;
            set => obstacles = value;
        }

        public List<GameObject> Fruits
        {
            get => fruits;
            set => fruits = value;
        }
        
        public Tile[,] Tiles
        {
            get => tiles;
            set => tiles = value;
        }

        public Field(int fieldWidth, int fieldHeight, int minObstaclesAmount, int maxObstaclesAmount, int fruitAmount)
        {
            this.fieldWidth = fieldWidth;
            this.fieldHeight = fieldHeight;
            this.minObstaclesAmount = minObstaclesAmount;
            this.maxObstaclesAmount = maxObstaclesAmount;
            this.fruitAmount = fruitAmount;

            obstacles = new List<GameObject>();
            fruits = new List<GameObject>();

            tiles = new Tile[fieldWidth, fieldHeight];
            for (int i = 0; i < fieldWidth; i++)
            {
                for (int j = 0; j < fieldHeight; j++)
                {
                    tiles[i, j] = new Tile(new Vector2Int(i,j));
                }
            }
            random = new Random();

            GenerateObstacles();
            //DrawField();
        }
        

        public void InitializeField()
        {
            
        }
        
        public void GenerateFruit() 
        { 
            for (int i = 0; i < fruitAmount; i++)
            {
                int row = random.Next(0, fieldHeight); 
                int column = random.Next(0, fieldWidth);
                if (tiles[column, row].TileType != TileType.Empty)
                {
                    i--;
                    continue;
                }
                tiles[column, row].TileType = TileType.Fruit;
                OnGenerateFruit.Invoke(tiles[column, row]);
            }
        }

        public void GenerateObstacles()
        {
            int obstaclesAmount = random.Next(minObstaclesAmount, maxObstaclesAmount);
            for (int i = 0; i < obstaclesAmount; i++)
            {
                int row = random.Next(0, fieldHeight); 
                int column = random.Next(0, fieldWidth);
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