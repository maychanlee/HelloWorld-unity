using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance;

    private HashSet<string> completedMinigames = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void MarkMinigameComplete(MinigameKey key)
    {
        completedMinigames.Add(GetKey(key));
        Debug.Log($"Marked complete: {key}");
    }

    public bool IsMinigameComplete(int neighborId, int minigameId)
    {
        return completedMinigames.Contains(GetKey(neighborId, minigameId));
    }

    private string GetKey(MinigameKey key)
        => GetKey(key.neighborId, key.minigameId);

    private string GetKey(int neighborId, int minigameId)
        => $"{neighborId}_{minigameId}";

    public int GetCompletedCount()
    {
        return completedMinigames.Count;
    }

    public int GetTotalMinigameCount()
    {
        return 6; // 2 neighbors x 3 minigames

        
    }

    private HashSet<int> learnedGreetingFromNeighbor = new HashSet<int>();

    public bool HasLearnedGreeting(int neighborId)
    {
        return learnedGreetingFromNeighbor.Contains(neighborId);
    }

    public void MarkGreetingLearned(int neighborId)
    {
        learnedGreetingFromNeighbor.Add(neighborId);
}


}
