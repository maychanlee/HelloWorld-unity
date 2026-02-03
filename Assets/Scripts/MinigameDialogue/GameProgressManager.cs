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

    public List<MinigameSaveEntry> GetCompletedMinigamesForSave()
    {
        List<MinigameSaveEntry> list = new List<MinigameSaveEntry>();

        foreach (string key in completedMinigames)
        {
            string[] parts = key.Split('_');
            list.Add(new MinigameSaveEntry
            {
                neighborId = int.Parse(parts[0]),
                minigameId = int.Parse(parts[1])
            });
        }

        return list;
    }

    public void LoadCompletedMinigames(List<MinigameSaveEntry> list)
    {
        completedMinigames.Clear();

        foreach (var entry in list)
            completedMinigames.Add($"{entry.neighborId}_{entry.minigameId}");
    }

    public List<int> GetLearnedGreetingsForSave()
    {
        return new List<int>(learnedGreetingFromNeighbor);
    }

    public void LoadLearnedGreetings(List<int> list)
    {
        learnedGreetingFromNeighbor = new HashSet<int>(list);
    }



}
