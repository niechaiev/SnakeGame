using System;
using System.Collections.Generic;

namespace Game
{
    public class Snake
    {
        private LinkedList<Tile> segments;
        private Tile head;
        private Field field;
        public Action<Tile, Tile> OnMove;
        public Action<Tile> OnGrow;

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

        public Snake(Tile position, Field field)
        {
            head = position;
            segments = new LinkedList<Tile>();
            AddSegment(head);
            this.field = field;
        }
        
        public bool CheckCrash(Tile nextTile) 
        {
            foreach (var tile in segments)
            {
                if (tile == nextTile)
                    return true;
            }
            return false; 
        } 

        public void AddSegment(Tile tile)
        {
            segments.AddLast(tile);
            tile.TileType = TileType.Snake;
        }
        public void Grow()
        {
            segments.AddLast(head);
            OnGrow.Invoke(head);
        }

        public void Move(Tile nextTile)
        {
            var tail = segments.Last.Value;
            segments.RemoveLast();
            tail.TileType = TileType.Empty;

            head = nextTile;
            head.TileType = TileType.Snake;
            segments.AddFirst(head);

            OnMove.Invoke(tail, head);
        }
    }
}