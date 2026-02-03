using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinigameSaveEntry
{
    public int neighborId;
    public int minigameId;
}

[System.Serializable]
public class SaveData
{
    // =========================
    // PLAYER
    // =========================
    public Vector3 playerPosition;
    public int hungerLevel;

    // =========================
    // WORLD
    // =========================
    public string mapBoundaryName;

    // =========================
    // PROGRESSION
    // =========================
    public List<MinigameSaveEntry> completedMinigames = new List<MinigameSaveEntry>();
    public List<int> learnedGreetings = new List<int>();

    // Optional but VERY useful
    public Dictionary<int, int> neighborMinigameIndex = new Dictionary<int, int>();
}
