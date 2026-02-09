using UnityEngine;

[CreateAssetMenu(menuName = "Config/Weed Game Config")]
public class WeedGameConfig : ScriptableObject
{
    [Header("Grid Settings")]
    public int gridWidth = 12;
    public int gridHeight = 11;
    public float cellSize = 1f;

    [Header("Player Settings")]
    public float gameplayMovementSpeed = 8f;
}
