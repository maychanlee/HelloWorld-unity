using System.Collections.Generic;
using UnityEngine;
public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance;

    private Dictionary<string, MinigameSaveEntry> completedMinigames =
        new Dictionary<string, MinigameSaveEntry>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private string GetKey(int neighborId, int minigameId)
        => $"{neighborId}_{minigameId}";

    public void MarkMinigameComplete(MinigameData data, float completionTime)
    {
        string dictKey = GetKey(data.neighborId, data.minigameId);

        completedMinigames[dictKey] = new MinigameSaveEntry
        {
            neighborId = data.neighborId,
            minigameId = data.minigameId,
            neighborName = data.neighborName,
            completionPhrase = data.completionPhrase,
            completionTime = completionTime,
        };
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

    public int GetCompletedMinigameCount()
    {
        return completedMinigames.Count;
    }

    public int GetTotalMinigameCount()
    {
        int total = 0;
        foreach (var npc in FindObjectsOfType<NPCQuestGiver>())
            total += npc.minigames.Count;

        return total;
    }
    public bool IsMinigameComplete(MinigameData data)
    {
        return IsMinigameComplete(data.neighborId, data.minigameId);
    }

    public MinigameSaveEntry GetMinigameResult(MinigameData data)
    {
        return GetMinigameResult(data.neighborId, data.minigameId);
    }


}
