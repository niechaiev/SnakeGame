using UnityEngine;

namespace Game
{
    public class Tile
    {
        private Vector2Int position;
        private TileType tileType;
        private GameObject tileObject;

        public GameObject TileObject
        {
            get => tileObject;
            set => tileObject = value;
        }

        public Vector2Int Position
        {
            get => position;
            set => position = value;
        }

        public TileType TileType
        {
            get => tileType;
            set => tileType = value;
        }

        public Tile()
        {
            position = Vector2Int.zero;
            tileType = TileType.Empty;
            
        }
        
        public Tile(Vector2Int position, GameObject tileObject, TileType tileType = TileType.Empty)
        {
            this.position = position;
            this.tileObject = tileObject;
            this.tileType = tileType;
        }
    }
}