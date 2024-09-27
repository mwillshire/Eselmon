//I do not like how many tiles are created through this script. Could try to only create enough tiles to cover the camera area then delete when they 
// go off camera and create them when they go on camera. That would significantly reduce the number of tiles.

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CombatGridManager : MonoBehaviour
{
    [Header("Inscribed")]
    public int width;
    public int height;
    public CombatGridTile tilePrefab;

    [Header("Dynamic")]
    public Vector2 bottomLeftCorner;

    private bool hasGenerated = false;
    private Vector2 prevCoord = Vector2.zero;

    private Dictionary<Vector2, CombatGridTile> tiles;

    private PlayerController pC;

    private void Start()
    {
        pC = FindFirstObjectByType<PlayerController>();
    }

    private void FixedUpdate()
    {
        bottomLeftCorner.y = Camera.main.transform.position.y - Camera.main.orthographicSize;
        bottomLeftCorner.x = Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect);

        bottomLeftCorner.y = UtilityMethods.RoundToNumber(bottomLeftCorner.y, 0.64f, StaticVariables.offset);
        bottomLeftCorner.x = UtilityMethods.RoundToNumber(bottomLeftCorner.x, 0.64f, StaticVariables.offset);

        if (StaticVariables.combatMode)
        {
            if (!hasGenerated || prevCoord != bottomLeftCorner)
            {
                GenerateGrid();
                hasGenerated = true;
            }
        }

        prevCoord.y = bottomLeftCorner.y;
        prevCoord.x = bottomLeftCorner.x;

        if (hasGenerated)
        {
            foreach (var tile in tiles)
            {
                if (tile.Value.isClicked)
                {
                    pC.TakeAction(tile.Value.gameObject.transform.position);
                    tile.Value.isClicked = false;
                }
            }
        }
    }
    private void GenerateGrid()
    {
        if (hasGenerated)
        {
            foreach (var tile in tiles)
            {
                Destroy(tile.Value.gameObject);
            }
            tiles.Clear();
        }

        tiles = new Dictionary<Vector2, CombatGridTile>();

        float xPos = 0.32f + bottomLeftCorner.x - 0.64f;
        float yPos = 0.32f + bottomLeftCorner.y + 2.24f; //Previous = -0.96f with height 18

        for ( float x = 0; x < width; x++ )
        {
            for ( int y = 0; y < height; y++ )
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(xPos, yPos), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y %2 != 0) || (y % 2 == 0 && x % 2 != 0);
                spawnedTile.Init(isOffset);

                tiles[new Vector2(xPos, yPos)] = spawnedTile;
                yPos += .64f;
            }
            xPos += 0.64f;
            yPos = 0.32f + bottomLeftCorner.y + 2.24f;
        }
    }

    public CombatGridTile GetTileFromPosition ( Vector2 position )
    {
        if (tiles.TryGetValue(position, out var tile)) return tile;

        return null;
    }
}
