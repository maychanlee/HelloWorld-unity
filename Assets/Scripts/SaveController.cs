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

    public void SaveGame()
    {
        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        SaveData saveData = new SaveData
        {
            id = SlotId,
            createdAt = DateTime.UtcNow.ToString("r"),
            lastSaved = DateTime.UtcNow.ToString("r"),
            player = new PlayerSaveData()
        };

        // Player identity
        saveData.player.saveslotId = SlotId;

        // Player state
        saveData.player.playerPosition =
            playerObj.transform.position;

        saveData.player.hungerLevel =
            playerObj.GetComponent<PlayerHunger>()
                .currentHunger;

        // World state
        CinemachineConfiner confiner =
            FindObjectOfType<CinemachineConfiner>();

        saveData.player.mapBoundary =
            confiner.m_BoundingShape2D
                .gameObject.name;

        // Progression
        GameProgressManager progress =
            GameProgressManager.Instance;

        saveData.player.completedMinigames =
            progress.GetCompletedMinigamesForSave();


        File.WriteAllText(
            SaveFilePath,
            JsonUtility.ToJson(saveData, true)
        );

        Debug.Log($"Local save written to {SaveFilePath}");

        UploadPlayerSave(saveData.player);
    }

    private void UploadPlayerSave(PlayerSaveData playerData)
    {
        ApiClient api =
            FindObjectOfType<ApiClient>();

        int id = playerData.saveslotId;

        string url =
            $"{api.BackendBaseUrl}/saveslot/{id}/player/{id}";

        string json =
            JsonUtility.ToJson(playerData, true);

        Debug.Log($"Uploading player save to {url}");
        Debug.Log(json);

        StartCoroutine(
            api.SendJson(
                url,
                json,
                "PATCH",
                response =>
                {
                    Debug.Log("Remote player save successful");
                    Debug.Log(response);
                },
                () =>
                {
                    Debug.LogError("Remote player save FAILED");
                }
            )
        );
    }

    public void LoadGame()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.Log("No local save found, creating new save.");
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

    private void ApplySaveData(SaveData saveData)
    {
        if (saveData == null || saveData.player == null)
        {
            Debug.LogError("Invalid save data");
            return;
        }

        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        // Player state
        playerObj.transform.position =
            saveData.player.playerPosition;

        playerObj.GetComponent<PlayerHunger>()
            .currentHunger =
            saveData.player.hungerLevel;

        // World state
        CinemachineConfiner confiner =
            FindObjectOfType<CinemachineConfiner>();

        confiner.m_BoundingShape2D =
            GameObject.Find(saveData.player.mapBoundary)
                .GetComponent<PolygonCollider2D>();

        // Progression
        GameProgressManager.Instance
            .LoadCompletedMinigames(
                saveData.player.completedMinigames
            );
    }
}
