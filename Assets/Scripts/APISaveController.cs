using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Cinemachine;

public class APISaveController : MonoBehaviour
{
    public static APISaveController Instance;

    [Header("Backend Config")]
    [SerializeField] private BackendConfig backendConfig;

    [Header("Local Save")]
    [SerializeField] private string saveFileName = "saveData.json";

    public SaveData CurrentSave { get; private set; }
    public int SlotId { get; private set; }
    public int PlayerId { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (backendConfig == null)
            Debug.LogError("BackendConfig not assigned on APISaveController");
    }

    // =========================
    // MAIN MENU ENTRY POINTS
    // =========================

    public void NewSlot(int slotId)
    {
        SlotId = slotId;
        PlayerId = slotId;

        CurrentSave = CreateDefaultSave();
        SceneManager.LoadScene("GameScene");
    }

    public void LoadSlot(int slotId)
    {
        SlotId = slotId;
        PlayerId = slotId;

        StartCoroutine(LoadWithFallback());
    }

    // =========================
    // LOAD LOGIC
    // =========================

    private IEnumerator LoadWithFallback()
    {
        // 1️⃣ Remote API
        yield return TryLoadFromApi(backendConfig.remoteBaseUrl);
        if (CurrentSave != null)
        {
            SceneManager.LoadScene("GameScene");
            yield break;
        }

        // 2️⃣ Local API
        yield return TryLoadFromApi(backendConfig.localBaseUrl);
        if (CurrentSave != null)
        {
            SceneManager.LoadScene("GameScene");
            yield break;
        }

        // 3️⃣ Disk fallback
        LoadFromDisk();
        SceneManager.LoadScene("GameScene");
    }

    private IEnumerator TryLoadFromApi(string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            yield break;

        string url = $"{baseUrl}/{SlotId}/player/{PlayerId}";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.timeout = backendConfig.timeoutSeconds;
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success ||
                string.IsNullOrEmpty(req.downloadHandler.text))
            {
                yield break;
            }

            CurrentSave =
                JsonUtility.FromJson<SaveData>(req.downloadHandler.text);
        }
    }

    private void LoadFromDisk()
    {
        string path = GetSavePath();

        if (!File.Exists(path))
        {
            CurrentSave = CreateDefaultSave();
            return;
        }

        string json = File.ReadAllText(path);
        CurrentSave = JsonUtility.FromJson<SaveData>(json);
    }

    // =========================
    // APPLY SAVE (GAME SCENE)
    // =========================

    public void ApplyLoadedSave()
    {
        if (CurrentSave == null)
        {
            Debug.LogWarning("No save data to apply.");
            return;
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found.");
            return;
        }

        player.transform.position = CurrentSave.playerPosition;
        player.GetComponent<PlayerHunger>().currentHunger =
            CurrentSave.hungerLevel;

        FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D =
            GameObject.Find(CurrentSave.mapBoundary)
                .GetComponent<PolygonCollider2D>();

        GameProgressManager.Instance
            .LoadCompletedMinigames(CurrentSave.completedMinigames);
    }

    // =========================
    // SAVE LOGIC
    // =========================

    public void SaveGame()
    {
        CurrentSave = BuildSaveData();
        StartCoroutine(SaveWithFallback(CurrentSave));
        Debug.Log("Game saved successfully.");
        Debug.Log("Save file path: " + GetSavePath());
    }

    private IEnumerator SaveWithFallback(SaveData save)
    {
        bool saved = false;

        yield return TrySaveToApi(backendConfig.remoteBaseUrl, save, () => saved = true);
        if (!saved)
            yield return TrySaveToApi(backendConfig.localBaseUrl, save, () => saved = true);

        SaveToDisk(save);
    }

    private IEnumerator TrySaveToApi(
        string baseUrl,
        SaveData save,
        System.Action onSuccess)
    {
        if (string.IsNullOrEmpty(baseUrl))
            yield break;

        string url = $"{baseUrl}/{SlotId}/player/{PlayerId}";
        string json = JsonUtility.ToJson(save);

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler =
            new UploadHandlerRaw(
                System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        req.timeout = backendConfig.timeoutSeconds;

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke();
    }

    private void SaveToDisk(SaveData save)
    {
        string path = GetSavePath();
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, JsonUtility.ToJson(save, true));
    }

    // =========================
    // HELPERS
    // =========================

    private SaveData BuildSaveData()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found in BuildSaveData()");
            return new SaveData();
        }

        SaveData save = new SaveData
        {
            playerPosition = player.transform.position,
            hungerLevel = player.GetComponent<PlayerHunger>().currentHunger,
            mapBoundary = FindObjectOfType<CinemachineConfiner>()
                ?.m_BoundingShape2D?.gameObject?.name ?? "DefaultBoundary",
            completedMinigames = new List<MinigameSaveEntry>()
        };

        List<MinigameSaveEntry> completed = 
            GameProgressManager.Instance.GetCompletedMinigamesForSave();

        foreach (var entry in completed)
        {
            save.completedMinigames.Add(entry);
        }

        Debug.Log($"BuildSaveData: {save.completedMinigames.Count} minigames included.");

        return save;
    }


    private SaveData CreateDefaultSave()
    {
        return new SaveData
        {
            playerPosition = Vector3.zero,
            hungerLevel = 50,
            mapBoundary = "Neighborhood"
        };
    }

    private string GetSavePath()
    {
        return Path.Combine(
            Application.persistentDataPath,
            $"slot_{SlotId}",
            saveFileName);
    }
}
