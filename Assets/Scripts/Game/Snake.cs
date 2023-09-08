using System;
using System.Collections.Generic;

namespace Game
{
    public class Snake
    {
        private readonly int startingLength = 7;
        private readonly float growSpeedGain = 0.05f;
        private readonly int maxLength = 10;
        private float speed = 0.5f;
        public int StartingLength => startingLength;
        public float GrowSpeedGain => growSpeedGain;
        public int MaxLength => maxLength;

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        private LinkedList<Tile> segments;
        private Tile head;
        private Action<Tile, Tile> onMove;
        private Action<Tile> onGrow;
        private Action<int> onSizeChanged;
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

        public Snake(Tile position, Action<Tile, Tile> onMove, Action<Tile> onGrow, Action<int> onSizeChanged)
        {
            head = position;
            segments = new LinkedList<Tile>();
            AddSegment(head);

            this.onMove += onMove;
            this.onGrow += onGrow;
            this.onSizeChanged = onSizeChanged;
        }

        public void UnSubscribe(Action<Tile, Tile> onMove,
            Action<Tile> onGrow)
        {
            this.onMove -= onMove;
            this.onGrow -= onGrow;
        }

        public bool CheckSelfCrash(Tile nextTile)
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
            onSizeChanged?.Invoke(segments.Count);
        }

        public void Grow(Tile nextTile)
        {
            nextTile.TileType = TileType.Snake;
            segments.AddFirst(nextTile);
            head = nextTile;
            onGrow.Invoke(nextTile);
            onSizeChanged?.Invoke(segments.Count);
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