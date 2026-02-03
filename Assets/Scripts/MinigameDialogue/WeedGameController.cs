using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class WeedGameController : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 12;
    [SerializeField] private int gridHeight = 11;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector2 gridOffset = Vector2.zero;
    [SerializeField] private bool useManualStartPos = false;
    [SerializeField] private Vector2 manualStartPos = Vector2.zero;
    
    [Header("Minigame Identity")]
    [SerializeField] private int neighborId;
    [SerializeField] private int minigameId;
    private MinigameKey currentKey;

    [Header("Completion State")]
    [SerializeField] private bool isComplete = false;
    public bool IsComplete => isComplete;


    [Header("Weed Settings")]
    [SerializeField] private GameObject weedPrefab;
    private int totalWeeds; // 12 x 11 = 132
    private int remainingWeeds;
    
    [Header("Player Settings")]
    [SerializeField] private GameObject player;
    [SerializeField] private float gameplayMovementSpeed = 8f;
    [SerializeField] private Transform playerStartPosition; // Center of the grid
    
    [Header("Timer")]
    private float gameTimer = 0f;
    private bool isGameActive = false;
    
    [Header("Teleport & Rewards")]
    public PolygonCollider2D mapBoundary;
    public PolygonCollider2D returnMapBoundary;
    CinemachineConfiner confiner;
    public Transform neighborHousePosition;
    public UnityEvent onGameComplete;
    
    [Header("UI References (Optional)")]
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    [SerializeField] private TMPro.TextMeshProUGUI weedCountText;
    
    private PlayerMovement playerMovement; // Reference to player movement script
    private float originalPlayerSpeed; // Store original speed to restore later
    private List<GameObject> weedList = new List<GameObject>();
    
    private void Awake()
    {
        // Initialize totalWeeds based on grid dimensions
        totalWeeds = gridWidth * gridHeight;
        remainingWeeds = totalWeeds;
        confiner = FindObjectOfType<CinemachineConfiner>();
        // Get player movement component
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
    }
    
    // Call this method when the player accepts the quest
    public void StartWeedGame(int neighborId, int minigameId)
    {
        Debug.Log($"Starting Weed Game: Neighbor {neighborId} - Minigame {minigameId}");

        if (player == null) Debug.LogError("Player is NULL");
        if (weedPrefab == null) Debug.LogError("Weed Prefab is NULL");
        if (playerStartPosition == null) Debug.LogError("Player Start Position is NULL");
        if (mapBoundary == null) Debug.LogError("Map Boundary is NULL");
        if (confiner == null) Debug.LogError("Cinemachine Confiner is NULL");
        
        this.neighborId = neighborId;
        this.minigameId = minigameId;

        currentKey = new MinigameKey(neighborId, minigameId);

        Debug.Log($"Starting Weed Game: {currentKey}");
        isComplete = false;


        // Store original player speed
        if (playerMovement != null)
        {
            originalPlayerSpeed = playerMovement.moveSpeed;
            playerMovement.moveSpeed = gameplayMovementSpeed;
        }

        TeleportPlayerToGameStart();
        confiner.m_BoundingShape2D = mapBoundary;

        GenerateWeedGrid();

        isGameActive = true;
        gameTimer = 0f;
        remainingWeeds = totalWeeds;

        UpdateUI();
    }

    
    private void GenerateWeedGrid()
    {
        // Clear any existing weeds
        foreach (GameObject weed in weedList)
        {
            if (weed != null)
            {
                Destroy(weed);
            }
        }
        weedList.Clear();
        
        // Calculate starting position (bottom-left corner)
        Vector2 startPos;
        
        if (useManualStartPos)
        {
            // Use manually specified start position
            startPos = manualStartPos;
        }
        else
        {
            // Calculate from grid offset (centered)
            startPos = gridOffset - new Vector2((gridWidth * cellSize) / 2f, (gridHeight * cellSize) / 2f);
        }
        
        // Generate grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 spawnPosition = startPos + new Vector2(x * cellSize + cellSize / 2f, y * cellSize + cellSize / 2f);
                GameObject weed = Instantiate(weedPrefab, spawnPosition, Quaternion.identity);
                weed.transform.SetParent(transform); // Organize under this manager
                
                // Set up the weed to notify this manager when destroyed
                Weed weedScript = weed.GetComponent<Weed>();
                if (weedScript != null)
                {
                    weedScript.SetManager(this);
                }
                
                weedList.Add(weed);
            }
        }
        
        Debug.Log($"Generated {weedList.Count} weeds in a {gridWidth}x{gridHeight} grid starting at {startPos}");
    }
    
    
    private void TeleportPlayerToGameStart()
    {
        if (player != null)
        {
            // Calculate center position
            player.transform.position = playerStartPosition.position;
        }
    }
    
    public void OnWeedDestroyed()
    {
        if (!isGameActive) return;
        
        remainingWeeds--;
        UpdateUI();
        
        Debug.Log($"Weed destroyed! Remaining: {remainingWeeds}");
        
        if (remainingWeeds <= 0)
        {
            CompleteGame();
            confiner.m_BoundingShape2D = returnMapBoundary;
            Debug.Log("Weed removal game completed! gameTimer: " + gameTimer);
        }
    }
    
    private void Update()
    {
        if (isGameActive)
        {
            gameTimer += Time.deltaTime;
            UpdateUI();
        }
    }
    
    private void UpdateUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(gameTimer / 60f);
            int seconds = Mathf.FloorToInt(gameTimer % 60f);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
        
        if (weedCountText != null)
        {
            weedCountText.text = $"Weeds: {remainingWeeds}/{totalWeeds}";
        }
    }
    
    private void CompleteGame()
    {
        isGameActive = false;
        isComplete = true;

        
        // Log completion time
        int minutes = Mathf.FloorToInt(gameTimer / 60f);
        int seconds = Mathf.FloorToInt(gameTimer % 60f);
        int milliseconds = Mathf.FloorToInt((gameTimer * 100f) % 100f);
        
        Debug.Log($"WEED GAME COMPLETED! Time: {minutes:00}:{seconds:00}.{milliseconds:00}");
        Debug.Log($"COMPLETED {currentKey} in {GetFormattedTime()}");

        
        // Restore original player speed
        if (playerMovement != null)
        {
            playerMovement.moveSpeed = originalPlayerSpeed;
        }
        
        // Teleport player back to neighbor's house
        StartCoroutine(CompleteGameSequence());
    }
    
    private IEnumerator CompleteGameSequence()
    {
        // Optional: Add a short delay before teleporting
        yield return new WaitForSeconds(1f);
        
        // Teleport player back
        if (player != null)
        {
            player.transform.position = neighborHousePosition.position;
            Debug.Log("Player teleported back to neighbor's house");
        }

        GameProgressManager.Instance.MarkMinigameComplete(
            currentKey,
            gameTimer,
            GetFormattedTime()
        );


        // Trigger reward sequence
        onGameComplete?.Invoke();
        
        // Optional: Clean up weeds
        foreach (GameObject weed in weedList)
        {
            if (weed != null)
            {
                Destroy(weed);
            }
        }
        weedList.Clear();
    }
    
    // Public method to get current game time (useful for rewards based on completion time)
    public float GetCompletionTime()
    {
        return gameTimer;
    }
    
    // Public method to format time as string
    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(gameTimer / 60f);
        int seconds = Mathf.FloorToInt(gameTimer % 60f);
        int milliseconds = Mathf.FloorToInt((gameTimer * 100f) % 100f);
        return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }
}