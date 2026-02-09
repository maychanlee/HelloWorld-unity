// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;

// public class MenuButton : MonoBehaviour
// {
//     [SerializeField] private int slotId;
//     [SerializeField] private int playerId;

//     public void NewGame()
//     {
//         GameSession.Instance.playerId = playerId;
//         GameSession.Instance.slotId = slotId;

//         // Create default save data
//         GameSession.Instance.currentSave = CreateDefaultSave();

//         SceneManager.LoadScene("GameScene");
//     }

//     public void ContinueGame()
//     {
//         GameSession.Instance.playerId = playerId;
//         GameSession.Instance.slotId = slotId;

//         StartCoroutine(LoadFromAPI());
//     }

//     private SaveData CreateDefaultSave()
//     {
//         return new SaveData
//         {
//             playerPosition = Vector2.zero,
//             hungerLevel = 50,
//             mapBoundary = "Neighborhood",
//         };
//     }

//     private IEnumerator LoadFromAPI()
//     {
//         string url =
//             $"https://yourapi.com/save?playerId={playerId}&slotId={slotId}";

//         using (UnityEngine.Networking.UnityWebRequest req =
//                UnityEngine.Networking.UnityWebRequest.Get(url))
//         {
//             yield return req.SendWebRequest();

//             if (req.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
//             {
//                 Debug.LogError(req.error);
//                 yield break;
//             }

//             SaveData save =
//                 JsonUtility.FromJson<SaveData>(req.downloadHandler.text);

//             GameSession.Instance.currentSave = save;

//             SceneManager.LoadScene("GameScene");
//         }
//     }
// }
