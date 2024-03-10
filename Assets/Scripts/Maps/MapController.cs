using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] private int cellSize = 12;
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject mapSectionPrefab;

    private void Start()
    {
        MaskBorders();
    }

    [ContextMenu("Create Grid")]
    public void CreateGrid()
    {
        if (gridParent.childCount > 0)
        {
            GameObject ngo = new GameObject();
            ngo.transform.parent = transform;
            ngo.transform.position = gridParent.position;
            ngo.layer = LayerMask.NameToLayer("Map");

            if (Application.isEditor && !Application.isPlaying)
                DestroyImmediate(gridParent.gameObject);
            else
                Destroy(gameObject.gameObject);

            ngo.name = "GRID PARENT";
            gridParent = ngo.transform;
        }


        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                SpawnSection(x, y);
            }
        }
    }

    private void SpawnSection(int x, int y)
    {
        Vector3 offset = new Vector3();
        //offset = new Vector3(-cellSize * (mapSize / 2), cellSize * (mapSize / 2));
        GameObject go = Instantiate(mapSectionPrefab, gridParent);
        go.name = go.name.Replace("(Clone)", "") + $" <{x}, {y}>";
        go.transform.position = new Vector3(gridParent.position.x + (x * cellSize),
            gridParent.position.y - (y * cellSize)) + offset;
        go.transform.localScale = new Vector3(cellSize, cellSize, 1);
        go.layer = LayerMask.NameToLayer("Map");
        go.SetActive(true);
    }

    public void MaskBorders()
    {
        var tilemaps = GetComponentsInChildren<TilemapRenderer>();

        for (int i = 0; i < tilemaps.Length; i++)
        {
            if (tilemaps[i].name.Contains("Visible") || tilemaps[i].CompareTag("Visible Map"))
                tilemaps[i].maskInteraction = SpriteMaskInteraction.None;
            else
                tilemaps[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            var tilemapComp = tilemaps[i].GetComponent<Tilemap>();
            tilemapComp.color = new Color(tilemapComp.color.r, tilemapComp.color.r, tilemapComp.color.b, 1);
        }
    }

    private void OnDrawGizmos()
    {
        float size = 10.0f;
        float halfSize = size * 0.5f;

        for (int i = 0; i < gridParent.childCount; i++)
        {
            Transform childTransform = gridParent.GetChild(i).transform;
            Vector3 topLeft = childTransform.position + new Vector3(-halfSize, halfSize, 0);
            Vector3 topRight = childTransform.position + new Vector3(halfSize, halfSize, 0);
            Vector3 bottomRight = childTransform.position + new Vector3(halfSize, -halfSize, 0);
            Vector3 bottomLeft = childTransform.position + new Vector3(-halfSize, -halfSize, 0);
            
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}