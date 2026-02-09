using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class WeedGameController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private WeedGameConfig config;
    
    [Header("Minigame Identity")]
    [SerializeField] private MinigameData currentMinigame;

    [Header("Completion State")]
    [SerializeField] private bool isComplete = false;
    public bool IsComplete => isComplete;


    [Header("Weed Settings")]
    [SerializeField] private GameObject weedPrefab;
    private int totalWeeds;
    private int remainingWeeds;
    private List<GameObject> weedList = new List<GameObject>();
    
    [Header("Player Settings")]
    [SerializeField] private GameObject player;
    private PlayerMovement playerMovement;
    private float originalPlayerSpeed;
    [SerializeField] private Transform playerStartPosition; 
    
    [Header("Timer")]
    private float gameTimer = 0f;
    private bool isGameActive = false;
    
    [Header("Teleport & Rewards")]
    public PolygonCollider2D mapBoundary;
    public PolygonCollider2D returnMapBoundary;
    CinemachineConfiner confiner;
    public Transform neighborHousePosition;
    public UnityEvent onGameComplete;
    
    [Header("UI References")]
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    [SerializeField] private TMPro.TextMeshProUGUI weedCountText;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject hungerSlider;
    [SerializeField] private GameObject progressSlider;
    
    
    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner>();

        // Get player movement component
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }

        // Hide the game UI initially
        if (uiPanel != null)
            uiPanel.SetActive(false);

        // Show hunger slider initially
        if (hungerSlider != null)
            hungerSlider.SetActive(true);

        // Show progress slider initially
        if (progressSlider != null)
            progressSlider.SetActive(true);
    }
    
    public void StartWeedGame(MinigameData minigameData)
    {
        if (minigameData == null)
        {
            Debug.LogError("MinigameData is NULL");
            return;
        }
        if (minigameData.weedConfig == null)
        {
            Debug.LogError($"WeedGameConfig missing on {minigameData.name}");
            return;
        }

        currentMinigame = minigameData;
        config = minigameData.weedConfig;

        ApplyConfig();

        isComplete = false;

        // Set player speed for the game
        if (playerMovement != null)
        {
            originalPlayerSpeed = playerMovement.moveSpeed;
            playerMovement.moveSpeed = config.gameplayMovementSpeed;
        }

        TeleportPlayerToGameStart();
        confiner.m_BoundingShape2D = mapBoundary;

        GenerateWeedGrid();

        isGameActive = true;
        gameTimer = 0f;
        remainingWeeds = totalWeeds;

        // Show game UI
        if (uiPanel != null)
            uiPanel.SetActive(true);

        // Hide hunger slider
        if (hungerSlider != null)
            hungerSlider.SetActive(false);

        // Hide progress slider
        if (progressSlider != null)
            progressSlider.SetActive(false);

        UpdateUI();
    }

    private void ApplyConfig()
    {
        totalWeeds = config.gridWidth * config.gridHeight;
    }
    
    private void GenerateWeedGrid()
    {
        // Remove existing weeds if any
        foreach (GameObject weed in weedList)
        {
            if (weed != null)
            {
                Destroy(weed);
            }
        }
        weedList.Clear();
        // Validate player start position
        if (playerStartPosition == null)
        {
            Debug.LogError("Player start position not set!");
            return;
        }

        // Calculate center of the grid based on player start position
        Vector2 center = playerStartPosition.position;
        float gridWorldWidth  = config.gridWidth * config.cellSize;
        float gridWorldHeight = config.gridHeight * config.cellSize;
        // Calculate bottom-left corner of the grid
        Vector2 bottomLeft = center - new Vector2(gridWorldWidth / 2f, gridWorldHeight / 2f);

        for (int x = 0; x < config.gridWidth; x++)
        {
            for (int y = 0; y < config.gridHeight; y++)
            {
                Vector2 spawnPosition = bottomLeft
                    + new Vector2(
                        x * config.cellSize + config.cellSize / 2f,
                        y * config.cellSize + config.cellSize / 2f
                    );

                GameObject weed = Instantiate(weedPrefab, spawnPosition, Quaternion.identity, transform);

                Weed weedScript = weed.GetComponent<Weed>();
                if (weedScript != null)
                    weedScript.SetManager(this);

                weedList.Add(weed);
            }
        }

        Debug.Log(
            $"Generated {weedList.Count} weeds centered at {center} " +
            $"({config.gridWidth}x{config.gridHeight})"
        );
    }
    
    private void TeleportPlayerToGameStart()
    {
        if (player != null)
        {
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
            playerMovement.moveSpeed = config.gameplayMovementSpeed;
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

        
        // Restore original player speed
        if (playerMovement != null)
        {
            playerMovement.moveSpeed = originalPlayerSpeed;
        }
        
            // Hide game UI
        if (uiPanel != null)
            uiPanel.SetActive(false);

        // Show hunger slider again
        if (hungerSlider != null)
            hungerSlider.SetActive(true);

        // Show progress slider again
        if (progressSlider != null)
            progressSlider.SetActive(true);

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
            currentMinigame,
            gameTimer
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
}