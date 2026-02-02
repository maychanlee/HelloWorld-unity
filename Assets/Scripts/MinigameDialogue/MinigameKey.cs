using UnityEngine;
[System.Serializable]
public struct MinigameKey
{
    public int neighborId;
    public int minigameId;

    public MinigameKey(int neighborId, int minigameId)
    {
        this.neighborId = neighborId;
        this.minigameId = minigameId;
    }

    public override string ToString()
    {
        return $"Neighbor {neighborId} - Minigame {minigameId}";
    }
}
