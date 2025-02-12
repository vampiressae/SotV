using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public GameObject hexPrefab; // Assign in Inspector
    public int rows = 5;
    public int cols = 5;
    public float hexSize = 1f; // Size of hexagon

    private float width;
    private float height;

    void Start()
    {
        width = Mathf.Sqrt(3) * hexSize; // Horizontal spacing
        height = 1.5f * hexSize; // Vertical spacing
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                float x = col * width;
                float y = row * height;

                // Offset odd rows for staggered layout
                if (col % 2 == 1)
                    y += height / 2;

                Vector3 hexPosition = new Vector3(x, y, 0);
                Instantiate(hexPrefab, hexPosition, Quaternion.identity, transform);
            }
        }
    }
}
