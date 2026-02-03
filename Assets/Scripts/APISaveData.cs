using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinigameSaveEntry
{
    public int neighborId;
    public int minigameId;

    public float completionTime;     // raw seconds
    public string formattedTime;      // for UI
}


[System.Serializable]
public class SaveData
{
    // Player
    public Vector3 playerPosition;
    public int hungerLevel;

    // World
    public string mapBoundaryName;

    // Progression
    public List<MinigameSaveEntry> completedMinigames = new List<MinigameSaveEntry>();
    public List<int> learnedGreetings = new List<int>();
}
