using UnityEngine;

[CreateAssetMenu(menuName = "Config/Weed Game Config")]
public class WeedGameConfig : ScriptableObject
{
    [Header("Identity")]
    public int minigameId;

    [Header("Grid Settings")]
    public int gridWidth = 12;
    public int gridHeight = 11;
    public float cellSize = 1f;
    public Vector2 gridOffset = Vector2.zero;

    [Header("Start Position")]
    public bool useManualStartPos;
    public Vector2 manualStartPos;
}
