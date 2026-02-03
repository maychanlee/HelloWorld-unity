// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using Cinemachine;
// using UnityEngine;

// public class SaveController : MonoBehaviour
// {
//     private string saveFileName = "saveData.json";
//     private string saveFilePath;

//     void Start()
//     {
//         saveFilePath = System.IO.Path.Combine(Application.persistentDataPath, saveFileName);
//         Debug.Log("Save file path: " + saveFilePath);
//     }

//     public void SaveGame()
//     {
//         SaveData saveData = new SaveData();
//         saveData.playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
//         saveData.mapBoundary = FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name;
//         saveData.hungerLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHunger>().currentHunger;
        

//         File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData));
//         Debug.Log("Game saved successfully.");
//     }

//     public void LoadGame()
//     {
//         if (File.Exists(saveFilePath))
//         {
//             SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveFilePath));
//             GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
//             FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();
//             GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHunger>().currentHunger = saveData.hungerLevel; // Replace with actual hunger level
//             Debug.Log("Game loaded successfully.");
//         }
//         else
//         {
//             SaveGame();
//             Debug.Log("No save file found. Created a new save file.");
//         }
//     }
// }
