using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCQuestGiver : MonoBehaviour
{
    [Header("Quest Settings")]
    [SerializeField] private string questPrompt = "Can you help me remove all the weeds from my backyard?";
    [SerializeField] private WeedGameController weedGameController;
    
    [Header("Reward Settings")]
    [SerializeField] private int goldReward = 100;
    [SerializeField] private int experienceReward = 50;
    [SerializeField] private string rewardDialogue = "Thank you so much! Here's your reward.";
    
    [Header("UI (Optional)")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject yesButton;
    [SerializeField] private GameObject noButton;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onQuestAccepted;
    [SerializeField] private UnityEvent onQuestCompleted;
    
    private bool questActive = false;
    private bool questCompleted = false;
    
    private void Start()
    {
        // Subscribe to the game completion event
        if (weedGameController != null)
        {
            // This will be called when the weed game is completed
            weedGameController.onGameComplete.AddListener(OnWeedGameCompleted);
        }
        
        // Hide dialogue panel initially
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.E))
    {
        FindObjectOfType<NPCQuestGiver>().InteractWithNPC();
    }
}
    
    // Call this when player interacts with NPC
    public void InteractWithNPC()
    {
        if (questCompleted)
        {
            ShowDialogue("I already gave you the reward! Thanks again!");
            return;
        }
        
        if (questActive)
        {
            ShowDialogue("You're already on the quest! Go clear those weeds!");
            return;
        }
        
        // Show the quest prompt
        ShowQuestPrompt();
    }
    
    private void ShowQuestPrompt()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = questPrompt;
            
            // Show Yes/No buttons
            if (yesButton != null) yesButton.SetActive(true);
            if (noButton != null) noButton.SetActive(true);
        }
        else
        {
            // Fallback: just start the game
            Debug.Log("No dialogue UI set up. Auto-accepting quest.");
            OnQuestAccepted();
        }
    }
    
    public void OnYesClicked()
    {
        OnQuestAccepted();
        CloseDialogue();
    }
    
    public void OnNoClicked()
    {
        ShowDialogue("Maybe another time then!");
        StartCoroutine(CloseDialogueAfterDelay(2f));
    }
    
    private void OnQuestAccepted()
    {
        questActive = true;
        Debug.Log("Quest accepted! Starting weed game...");
        
        // Trigger custom event
        onQuestAccepted?.Invoke();
        
        // Start the weed removal game
        if (weedGameController != null)
        {
            weedGameController.StartWeedGame();
        }
    }
    
    private void OnWeedGameCompleted()
    {
        questActive = false;
        questCompleted = true;
        
        // Give rewards
        GiveRewards();
        
        // Show reward dialogue
        ShowRewardDialogue();
        
        // Trigger completion event
        onQuestCompleted?.Invoke();
    }
    
    private void GiveRewards()
    {
        // Get completion time
        string completionTime = "";
        if (weedGameController != null)
        {
            completionTime = weedGameController.GetFormattedTime();
        }
        
        Debug.Log($"=== QUEST COMPLETED ===");
        Debug.Log($"Completion Time: {completionTime}");
        Debug.Log($"Gold Reward: {goldReward}");
        Debug.Log($"Experience Reward: {experienceReward}");
        
        // TODO: Add actual reward logic here
        // Example: PlayerInventory.AddGold(goldReward);
        // Example: PlayerStats.AddExperience(experienceReward);
    }
    
    private void ShowRewardDialogue()
    {
        if (weedGameController != null)
        {
            string timeBonus = "";
            float time = weedGameController.GetCompletionTime();
            
            // Optional: Give bonus for fast completion
            if (time < 60f)
            {
                timeBonus = " You were so fast! Here's a bonus!";
                goldReward += 50; // Bonus gold
            }
            
            ShowDialogue($"{rewardDialogue}{timeBonus}\nCompleted in: {weedGameController.GetFormattedTime()}");
        }
        else
        {
            ShowDialogue(rewardDialogue);
        }
        
        StartCoroutine(CloseDialogueAfterDelay(4f));
    }
    
    private void ShowDialogue(string message)
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = message;
            
            // Hide Yes/No buttons for simple dialogue
            if (yesButton != null) yesButton.SetActive(false);
            if (noButton != null) noButton.SetActive(false);
        }
        else
        {
            Debug.Log($"NPC: {message}");
        }
    }
    
    private void CloseDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }
    
    private IEnumerator CloseDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CloseDialogue();
    }
    
    // Optional: Trigger interaction when player enters trigger zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // You could auto-show dialogue or wait for player to press a key
            Debug.Log("Player near NPC. Press E to interact.");
        }
    }
}