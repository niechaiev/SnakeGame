using UnityEngine;

namespace Game
{
    public class Field
    {
        
        private Tile[,] tiles;

        public Tile[,] Tiles
        {
            get => tiles;
            set => tiles = value;
        }

        public Field(int fieldWidth, int fieldHeight)
        {
            tiles = new Tile[fieldWidth, fieldHeight];
            for (int i = 0; i < fieldWidth; i++)
            {
                for (int j = 0; j < fieldHeight; j++)
                {
                    tiles[i, j] = new Tile(new Vector2Int(i,j));
                }
            }
            //DrawField();
        }
        

        public void InitializeField()
        {
            
        }


    }
}