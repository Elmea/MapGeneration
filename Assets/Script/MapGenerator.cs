using UnityEngine;

public static class BiomesColors
{
    public static Color water = new Color(0.16f, 0.45f, 0.91f);
    public static Color beach = new Color(0.91f, 0.75f, 0.32f);
    public static Color plain = new Color(0.45f, 0.91f, 0.16f);
    public static Color forest = new Color(0, 0.65f, 0);
    public static Color mountain = new Color(0.68f, 0.67f, 0.66f);
    public static Color snow = new Color(0.9f, 0.9f, 0.9f);
}

public class MapGenerator : MonoBehaviour
{
    Terrain terrain;
    Texture2D noiseTexture;

    private int width = 129, height = 129;
    public int depth;
    public float scale;
    public float seedX = 25;
    public float seedY = 25;

    float[,] currNoiseMap;

    [SerializeField] Material Material;

    public bool ColorizeTerrain;

    // Start is called before the first frame update
    void Start()
    {
        terrain = GetComponent<Terrain>();
        // planeMaterial = plane.GetComponent<Renderer>().sharedMaterial;
        // terrainMaterial = GetComponent<Renderer>().sharedMaterial;
        RegenerateTerrain();
    }

    void RegenerateTerrain()
    {
        currNoiseMap = Noise.GenerateNoiseMap(width, height, scale, seedX, seedY);
        GenerateTerrainData();
        GenerateTexture();
    }

    void GenerateTerrainData()
    {
        terrain.terrainData.heightmapResolution = width;

        terrain.terrainData.size = new Vector3(width, depth, height);
        terrain.terrainData.SetHeights(0, 0, currNoiseMap);
    }

    void GenerateTexture()
    {
        Color[] pixels = new Color[width * height];
        noiseTexture = new Texture2D(width, height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float mapHeight = currNoiseMap[x, y];
                pixels[x * height + y] = GetHeightColor(mapHeight);
            }
        }

        noiseTexture.SetPixels(pixels);
        noiseTexture.Apply();

        Material.mainTexture = noiseTexture;
    }

    Color GetHeightColor(float mapHeight)
    {
        if (!ColorizeTerrain)
            return new Color(mapHeight, mapHeight, mapHeight);

        if (mapHeight < 0.2f) return BiomesColors.water;
        else if (mapHeight < 0.3f) return BiomesColors.beach;
        else if (mapHeight < 0.4f) return BiomesColors.plain;
        else if (mapHeight < 0.6f) return BiomesColors.forest;
        else if (mapHeight < 0.85f) return BiomesColors.mountain;
        else return BiomesColors.snow;
    }

    // Update is called once per frame
    void Update()
    {
        RegenerateTerrain();
    }
}
