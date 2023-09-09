namespace Game
{
    public class Settings
    {
        struct Snake
        {
            private int startingLength;
            private float growSpeedGain;
            private int maxLength;
            private float speed;
        }

        struct Field
        {
            private int width;
            private int height;
            private int minObstaclesAmount;
            private int maxObstaclesAmount;
            private int fruitAmount;
        }
    }
}