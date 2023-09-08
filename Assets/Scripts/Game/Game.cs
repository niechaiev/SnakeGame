using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    public class Game
    {
        private Snake snake;
        private Field field;
        private Direction direction;

        public Game(Snake snake, Field field)
        {
            this.snake = snake;
            this.field = field;
            
            field.GenerateFruit();
        }

        public void Update()
        {
            var nextTile = GetNextTile(snake.Head);
            if (nextTile == null || snake.CheckCrash(nextTile))
                Debug.Log("gameover"); 
            
            
            if (nextTile.TileType == TileType.Fruit) { 
                snake.Grow(nextTile); 
                field.GenerateFruit(); 
                return;
            } 
            snake.Move(nextTile);
            
        }

        public void TurnLeft()
        {
            direction = EnumMod(--direction, 4);
            
        }

        public void TurnRight()
        {
            direction = EnumMod(++direction, 4);
        }
        
        private Direction EnumMod(Direction x, int m) {
            return (Direction)(((int)x%m + m)%m);
        }
       

        [CanBeNull]
        private Tile GetNextTile(Tile currentPosition)
        {
            int col = currentPosition.Position.x;
            int row = currentPosition.Position.y;

            switch (direction)
            {
                case Direction.Up:
                    row++;
                    break;
                case Direction.Right:
                    col++;
                    break;
                case Direction.Down:
                    row--;
                    break;
                case Direction.Left:
                    col--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (row < 0 || col < 0 || row >= field.Tiles.GetLength(0) || col >= field.Tiles.GetLength(1))
                return null;

            var nextTile = field.Tiles[col, row];
            return nextTile;
        }
    }
}