using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode] // Allows it to run in the editor
public class GridOutlineGenerator : MonoBehaviour
{
    public Tile dotTile; // Assign your dot tile here
    public Tilemap gridTilemap; // Assign your grid tilemap here
    public int width = 10; // Width of your grid in cells
    public int height = 10; // Height of your grid in cells
    public int cellSize = 10; // The size of your grid cells in units

    // Function to call for generating the grid outline
    public void GenerateGridOutline()
    {
        if (gridTilemap == null || dotTile == null)
        {
            Debug.LogError("Tilemap or Dot Tile not assigned.");
            return;
        }
        
        // Clear the tilemap before drawing the new grid outline
        gridTilemap.ClearAllTiles();

        // Loop over the grid size and set a tile only at the edges of each cell
        for (int x = 0; x <= width * cellSize; x += cellSize)
        {
            for (int y = 0; y <= height * cellSize; y += cellSize)
            {
                // Place dots along the vertical edges
                if (x < width * cellSize)
                {
                    gridTilemap.SetTile(new Vector3Int(x, y, 0), dotTile);
                }
                // Place dots along the horizontal edges
                if (y < height * cellSize)
                {
                    gridTilemap.SetTile(new Vector3Int(x, y, 0), dotTile);
                }
            }
        }

        // Loop to fill in the horizontal and vertical lines between the corners
        for (int x = 0; x <= width * cellSize; x += cellSize)
        {
            for (int y = 0; y < height * cellSize; y++)
            {
                if (y % cellSize != 0) // Avoid re-placing dots at the corners
                {
                    gridTilemap.SetTile(new Vector3Int(x, y, 0), dotTile);
                }
            }
        }
        for (int y = 0; y <= height * cellSize; y += cellSize)
        {
            for (int x = 0; x < width * cellSize; x++)
            {
                if (x % cellSize != 0) // Avoid re-placing dots at the corners
                {
                    gridTilemap.SetTile(new Vector3Int(x, y, 0), dotTile);
                }
            }
        }
    }

    // Custom editor method for convenience (requires using UnityEditor)
    [ContextMenu("Generate Grid Outline")]
    private void GenerateViaContextMenu()
    {
        GenerateGridOutline();
    }
}
