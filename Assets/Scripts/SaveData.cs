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
}

[System.Serializable]
public class PlayerSaveData
{
    public int saveslotId;

    public Vector3 playerPosition;
    public int hungerLevel;

    public string mapBoundary;

    public List<MinigameSaveEntry> completedMinigames =
        new List<MinigameSaveEntry>();
}

[System.Serializable]
public class SaveData
{
    public string createdAt;
    public int id;
    public string lastSaved;

    public PlayerSaveData player;
}
