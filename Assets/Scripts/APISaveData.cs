using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinigameSaveEntry
{
    public int neighborId;
    public string neighborName;
    public int minigameId;
    public string completionPhrase;
    public float completionTime;
    internal MinigameData minigameData;
}


[System.Serializable]
public class SaveData
{
    // Player
    public Vector3 playerPosition;
    public int hungerLevel;

    // World
    public string mapBoundary;

    // Progression
    public List<MinigameSaveEntry> completedMinigames = new List<MinigameSaveEntry>();
}
