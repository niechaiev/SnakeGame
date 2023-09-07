using System;
using System.Collections.Generic;

namespace Game
{
    public class Snake
    {
        private LinkedList<Tile> segments;
        private Tile head;
        private Field field;
        public Action<Tile, Tile> Swap;

        public LinkedList<Tile> Segments
        {
            get => segments;
            set => segments = value;
        }

        public Tile Head
        {
            get => head;
            set => head = value;
        }

        public Snake(Tile position, int startSize, Field field)
        {
            head = position;
            segments = new LinkedList<Tile>();
            segments.AddLast(head);
            position.TileType = TileType.Snake;
            this.field = field;
        }

        public void Grow()
        {
            segments.AddLast(head);
        }

        public void Move(Tile nextTile)
        {
            var tail = segments.Last.Value;
            segments.RemoveLast();
            tail.TileType = TileType.Empty;

            head = nextTile;
            head.TileType = TileType.Snake;
            segments.AddFirst(head);

            Swap(tail, head);
            

        }
    }
}