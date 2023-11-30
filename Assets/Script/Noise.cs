using UnityEngine;

public static class Noise
{   
    public static float[,] GenerateNoiseMap(int width, int height, float scale, float seedX, float seedY)
    {
        float[,] array = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = ((float)x + seedX) / width * scale;
                float yCoord = ((float)y + seedY) / height * scale;
                array[x, y] = Mathf.PerlinNoise(xCoord, yCoord);
            }
        }
        
        return array;
    }
}
