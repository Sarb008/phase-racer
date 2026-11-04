
using UnityEngine;
using UnityEditor;

public class TrackBuilder : EditorWindow
{
    private GameObject floorPrefab;
    private Transform roadParent;

    private const int TilesAcross = 5;
    private const int TilesForward = 20;
    private const float TileSize = 4f;

    [MenuItem("Phase Racer/Track Builder")]
    public static void OpenWindow()
    {
        GetWindow<TrackBuilder>("Track Builder");
    }

    private void OnGUI()
    {
        GUILayout.Label("PHASE RACER - Track Builder", EditorStyles.boldLabel);

        floorPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Floor Prefab", floorPrefab, typeof(GameObject), false);

        roadParent = (Transform)EditorGUILayout.ObjectField(
            "Road Parent", roadParent, typeof(Transform), true);

        if (GUILayout.Button("Generate Track"))
        {
            GenerateTrack();
        }
    }

    private void GenerateTrack()
    {
        if (floorPrefab == null || roadParent == null)
        {
            Debug.LogError("Assign the Floor Prefab and Road Parent first.");
            return;
        }

        // Keep generation deterministic and avoid stacking duplicate roads.
        if (roadParent.childCount > 0)
        {
            Debug.LogError(
                "Road Parent must be empty. Remove the test tile first.");
            return;
        }

        // Mesh bounds are X: -4 to 0, Z: -4 to 0.
        // Offset each prefab origin by +4 to align its mesh correctly.
        const float meshOffset = 4f;

        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Generate Phase Racer Track");

        for (int z = 0; z < TilesForward; z++)
        {
            for (int x = 0; x < TilesAcross; x++)
            {
                GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(
                    floorPrefab, roadParent);

                Undo.RegisterCreatedObjectUndo(tile, "Create Road Tile");

                float worldX = (x * TileSize) - 8f + meshOffset;
                float worldZ = (z * TileSize) + meshOffset;

                tile.transform.localPosition =
                    new Vector3(worldX, 0f, worldZ);

                tile.transform.localRotation = Quaternion.identity;
                tile.transform.localScale = Vector3.one;

                tile.name = $"RoadTile_{x}_{z}";
            }
        }

        Undo.CollapseUndoOperations(undoGroup);

        Debug.Log("PHASE RACER: 100 road tiles generated successfully.");
    }
}
