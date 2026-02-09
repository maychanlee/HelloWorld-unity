using System;
using System.IO;
using Cinemachine;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private int SlotId => SaveSlotManager.Instance.CurrentSlotId;

    private string SaveFilePath =>
        Path.Combine(
            Application.persistentDataPath,
            $"save_slot_{SlotId}.json"
        );

    /* =======================
     * SAVE
     * ======================= */
    public void SaveGame()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        SaveData saveData = new SaveData
        {
            id = SlotId,
            createdAt = DateTime.UtcNow.ToString("r"),
            lastSaved = DateTime.UtcNow.ToString("r"),
            player = new PlayerSaveData()
        };

        // Player identity (playerId == slotId)
        saveData.player.saveslotId = SlotId;

        // Player state
        saveData.player.playerPosition = playerObj.transform.position;
        saveData.player.hungerLevel =
            playerObj.GetComponent<PlayerHunger>().currentHunger;

        // World state
        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();
        saveData.player.mapBoundary =
            confiner.m_BoundingShape2D.gameObject.name;

        // Progression
        GameProgressManager progress = GameProgressManager.Instance;
        saveData.player.completedMinigames =
            progress.GetCompletedMinigamesForSave();

        // Local save
        File.WriteAllText(
            SaveFilePath,
            JsonUtility.ToJson(saveData, true)
        );

        Debug.Log($"Game saved to slot {SlotId}");
        Debug.Log($"Save file path: {SaveFilePath}");
    }

    /* =======================
     * LOAD
     * ======================= */
    public void LoadGame()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.Log("No save found for this slot. Creating new save.");
            SaveGame();
            return;
        }

        SaveData saveData =
            JsonUtility.FromJson<SaveData>(
                File.ReadAllText(SaveFilePath)
            );

        ApplySaveData(saveData);

        Debug.Log($"Game loaded from slot {SlotId}");

        foreach (var npc in FindObjectsOfType<NPCQuestGiver>())
        {
            npc.RefreshProgressFromSave();
        }
    }

    /* =======================
     * APPLY SAVE DATA
     * ======================= */
    private void ApplySaveData(SaveData saveData)
    {
        if (saveData == null || saveData.player == null)
        {
            Debug.LogError("SaveData or PlayerSaveData is null");
            return;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        // Player state
        playerObj.transform.position =
            saveData.player.playerPosition;
        playerObj.GetComponent<PlayerHunger>().currentHunger =
            saveData.player.hungerLevel;

        // World state
        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();
        confiner.m_BoundingShape2D =
            GameObject.Find(saveData.player.mapBoundary)
                .GetComponent<PolygonCollider2D>();

        // Progression
        GameProgressManager progress = GameProgressManager.Instance;
        progress.LoadCompletedMinigames(
            saveData.player.completedMinigames
        );
    }
}
