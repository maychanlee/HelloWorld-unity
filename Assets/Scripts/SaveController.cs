using System.IO;
using Cinemachine;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string SaveFilePath
    {
        get
        {
            int slotId = SaveSlotManager.Instance.CurrentSlotId;
            return Path.Combine(
                Application.persistentDataPath,
                $"save_slot_{slotId}.json"
            );
        }
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        saveData.playerPosition = player.transform.position;
        saveData.hungerLevel = player.GetComponent<PlayerHunger>().currentHunger;

        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();
        saveData.mapBoundary = confiner.m_BoundingShape2D.gameObject.name;

        GameProgressManager progress = GameProgressManager.Instance;
        saveData.completedMinigames = progress.GetCompletedMinigamesForSave();

        File.WriteAllText(SaveFilePath, JsonUtility.ToJson(saveData, true));
        Debug.Log($"Game saved to slot {SaveSlotManager.Instance.CurrentSlotId}");
    }

    public void LoadGame()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.Log("No save found for this slot. Creating new save.");
            SaveGame();
            return;
        }

        SaveData saveData =
            JsonUtility.FromJson<SaveData>(File.ReadAllText(SaveFilePath));

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = saveData.playerPosition;
        player.GetComponent<PlayerHunger>().currentHunger = saveData.hungerLevel;

        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();
        confiner.m_BoundingShape2D =
            GameObject.Find(saveData.mapBoundary)
            .GetComponent<PolygonCollider2D>();

        GameProgressManager progress = GameProgressManager.Instance;
        progress.LoadCompletedMinigames(saveData.completedMinigames);

        Debug.Log($"Game loaded from slot {SaveSlotManager.Instance.CurrentSlotId}");

        foreach (var npc in FindObjectsOfType<NPCQuestGiver>())
        {
            npc.RefreshProgressFromSave();
        }

    }
}
