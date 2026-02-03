using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using UnityEngine;

public class APISaveController : MonoBehaviour
{
    private string saveFileName = "saveData.json";
    private string saveFilePath;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
        Debug.Log("Save file path: " + saveFilePath);
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        var player = GameObject.FindGameObjectWithTag("Player");

        saveData.playerPosition = player.transform.position;
        saveData.hungerLevel = player.GetComponent<PlayerHunger>().currentHunger;

        saveData.mapBoundaryName =
            FindObjectOfType<CinemachineConfiner>()
            .m_BoundingShape2D.gameObject.name;

        var progress = GameProgressManager.Instance;
        saveData.completedMinigames = progress.GetCompletedMinigamesForSave();
        saveData.learnedGreetings = progress.GetLearnedGreetingsForSave();


        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData, true));
        Debug.Log("Game saved.");
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            SaveGame();
            return;
        }

        SaveData saveData =
            JsonUtility.FromJson<SaveData>(File.ReadAllText(saveFilePath));

        var player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = saveData.playerPosition;
        player.GetComponent<PlayerHunger>().currentHunger = saveData.hungerLevel;

        FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D =
            GameObject.Find(saveData.mapBoundaryName)
            .GetComponent<PolygonCollider2D>();

        var progress = GameProgressManager.Instance;
        progress.LoadCompletedMinigames(saveData.completedMinigames);
        progress.LoadLearnedGreetings(saveData.learnedGreetings);


        Debug.Log("Game loaded.");
    }
}
