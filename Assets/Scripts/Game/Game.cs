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
        }

        public void Update()
        {
        }

        [CanBeNull]
        private Tile GetNextCell(Tile currentPosition)
        {
            int row = currentPosition.Position.x;
            int col = currentPosition.Position.y;

            switch (direction)
            {
                case Direction.Up:
                    row--;
                    break;
                case Direction.Right:
                    col++;
                    break;
                case Direction.Down:
                    row++;
                    break;
                case Direction.Left:
                    col--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (row < 0 || col < 0 || row >= field.Tiles.GetLength(0) || col >= field.Tiles.GetLength(1))
                return null;

            var nextCell = field.Tiles[row, col];
            return nextCell;
        }
    }
}