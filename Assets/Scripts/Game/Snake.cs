using System.Collections.Generic;

namespace Game
{
    public class Snake
    {
        private LinkedList<Tile> segments;
        private Tile head;

        public Snake(Tile position, int startSize)
        {
            head = position;
            segments = new LinkedList<Tile>();
            segments.AddLast(head);
            position.TileType = TileType.Snake;
        }

        public void Grow()
        {
            segments.AddLast(head);
        }

        public void Move(Tile nextTile)
        {
            Tile tail = segments.Last.Value;
            segments.RemoveLast();
            tail.TileType = TileType.Empty;

            head = nextTile;
            segments.AddFirst(head);

        }
    }
}