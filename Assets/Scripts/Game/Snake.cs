using System;
using System.Collections.Generic;

namespace Game
{
    public class Snake
    {
        private LinkedList<Tile> segments;
        private Tile head;
        private Field field;
        private Action<Tile, Tile> onMove;
        private Action<Tile> onGrow;

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

        public Snake(Tile position, Field field,  Action<Tile, Tile> onMove,
        Action<Tile> onGrow)
        {
            head = position;
            segments = new LinkedList<Tile>();
            AddSegment(head);
            this.field = field;
            this.onMove += onMove;
            this.onGrow += onGrow;
        }

        public void UnSubscribe(Action<Tile, Tile> onMove,
            Action<Tile> onGrow)
        {
            this.onMove -= onMove;
            this.onGrow -= onGrow;
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
        public void Grow(Tile nextTile)
        {
            nextTile.TileType = TileType.Snake;
            segments.AddFirst(nextTile);
            head = nextTile;
            onGrow.Invoke(nextTile);
        }

        public void Move(Tile nextTile)
        {
            var tail = segments.Last.Value;
            segments.RemoveLast();
            tail.TileType = TileType.Empty;

            head = nextTile;
            head.TileType = TileType.Snake;
            segments.AddFirst(head);

            onMove.Invoke(tail, head);
        }
        
    }
}