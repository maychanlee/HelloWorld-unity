using System.Collections.Generic;
using UnityEngine;
public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance;

    private Dictionary<string, MinigameSaveEntry> completedMinigames =
        new Dictionary<string, MinigameSaveEntry>();

    private HashSet<int> learnedGreetingFromNeighbor = new HashSet<int>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private string GetKey(int neighborId, int minigameId)
        => $"{neighborId}_{minigameId}";

    public void MarkMinigameComplete(
        MinigameKey key,
        float completionTime,
        string formattedTime)
    {
        string dictKey = GetKey(key.neighborId, key.minigameId);

        completedMinigames[dictKey] = new MinigameSaveEntry
        {
            neighborId = key.neighborId,
            minigameId = key.minigameId,
            completionTime = completionTime,
            formattedTime = formattedTime
        };

        Debug.Log($"Marked complete: {dictKey} in {formattedTime}");
    }

    public bool IsMinigameComplete(int neighborId, int minigameId)
    {
        return completedMinigames.ContainsKey(GetKey(neighborId, minigameId));
    }

    public MinigameSaveEntry GetMinigameResult(int neighborId, int minigameId)
    {
        completedMinigames.TryGetValue(
            GetKey(neighborId, minigameId),
            out var entry);

        return entry;
    }

    public List<MinigameSaveEntry> GetCompletedMinigamesForSave()
    {
        return new List<MinigameSaveEntry>(completedMinigames.Values);
    }

    public void LoadCompletedMinigames(List<MinigameSaveEntry> list)
    {
        completedMinigames.Clear();

        foreach (var entry in list)
        {
            string key = GetKey(entry.neighborId, entry.minigameId);
            completedMinigames[key] = entry;
        }
    }

    public List<int> GetLearnedGreetingsForSave()
    {
        return new List<int>(learnedGreetingFromNeighbor);
    }

    public void LoadLearnedGreetings(List<int> list)
    {
        learnedGreetingFromNeighbor = new HashSet<int>(list);
    }

    public int GetCompletedMinigameCount()
    {
        return completedMinigames.Count;
    }

    // public int GetTotalMinigameCount()
    // {
    //     return FindObjectsOfType<NPCQuestGiver>()
    //         .Sum(npc => npc.minigames.Count);
    // }
    public int GetTotalMinigameCount()
    {
        // TEMP: hardcoded
        // Better: calculate from data later
        return 6; // 2 neighbors x 3 minigames
    }

}
