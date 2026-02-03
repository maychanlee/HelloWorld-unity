using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCQuestGiver : MonoBehaviour, IInteractable
{
    [Header("Identity")]
    public int neighborId;
    public string neighborName;

    [Header("Scene References (per neighbor)")]
    public PolygonCollider2D returnMapBoundary;
    public Transform neighborHousePosition;

    [Header("Minigames")]
    public List<MinigameData> minigames;
    private int currentMinigameIndex = 0;

    [Header("Systems")]
    public WeedGameController weedGameController;
    public DialogueManager dialogueManager;

    private bool playerInRange;
    private bool questActive;

    [Header("Player")]
    public PlayerHunger playerHunger;

    [SerializeField] private int hungerRequired = 10;


    // =========================
    // PLAYER INTERACTION
    // =========================

    
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    public void Interact()
    {
        HandleInteraction();
    }
    
    private void HandleInteraction()
    {
        if (questActive) return;

        PlayerHunger hunger = playerHunger != null
            ? playerHunger
            : FindObjectOfType<PlayerHunger>();

        if (hunger == null)
        {
            Debug.LogError("PlayerHunger not found in scene!");
            return;
        }

        if (hunger.currentHunger < 10)
        {
            dialogueManager.ShowLine(
                new DialogueLine { text = "You look hungry. Go eat something first." },
                onAutoAdvance: () => dialogueManager.HideDialogue()
            );
            return;
        }

        PlayPreGameDialogue();
    }

    // =========================
    // PRE-GAME DIALOGUE
    // =========================
    private void PlayPreGameDialogue()
    {
        var data = minigames[currentMinigameIndex];
        PlayDialogueSequence(data.preGameDialogue, 0, isPreGame: true);
    }

    // =========================
    // MINIGAME START
    // =========================
    private void StartMinigame()
    {
        questActive = true;

        var data = minigames[currentMinigameIndex];

        weedGameController.returnMapBoundary = returnMapBoundary;
        weedGameController.neighborHousePosition = neighborHousePosition;

        weedGameController.onGameComplete.RemoveAllListeners();
        weedGameController.onGameComplete.AddListener(OnMinigameCompleted);

        weedGameController.StartWeedGame(data.neighborId, data.minigameId);
    }

    // =========================
    // POST-GAME
    // =========================
    private void OnMinigameCompleted()
    {
        questActive = false;

        PlayerHunger hunger = FindObjectOfType<PlayerHunger>();

        if (hunger != null)
        {
            hunger.ChangeHunger(-10);
            Debug.Log("Hunger reduced by 10 after minigame.");
        }
        else
        {
            Debug.LogError("PlayerHunger not found when minigame completed!");
        }

        var data = minigames[currentMinigameIndex];
        currentMinigameIndex++;

        FindObjectOfType<MinigameProgressUI>()?.UpdateProgress();

        if (data.postGameDialogue != null)
        {
            PlayDialogueSequence(
                data.postGameDialogue,
                0,
                isPreGame: false,
                onComplete: () => dialogueManager.HideDialogue()
            );
        }
    }



    // =========================
    // DIALOGUE PLAYER
    // =========================
    private void PlayDialogueSequence(
        DialogueSequence seq,
        int index,
        bool isPreGame,
        UnityAction onComplete = null)
    {
        if (seq == null || index >= seq.lines.Count)
        {
            // Sequence finished
            onComplete?.Invoke();
            return;
        }

        DialogueLine line = seq.lines[index];

        dialogueManager.ShowLine(
            line,
            onYes: line.hasYesNo && isPreGame
                ? () =>
                {
                    StartMinigame();
                    dialogueManager.HideDialogue();
                }
                : null,
            onNo: line.hasYesNo && isPreGame
                ? () => dialogueManager.HideDialogue()
                : null,
            onAutoAdvance: () =>
                PlayDialogueSequence(seq, index + 1, isPreGame, onComplete)
        );
    }

    // =========================
    // TRIGGER ZONE
    // =========================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    private void PlayNotEnoughHungerDialogue()
    {
        var data = minigames[currentMinigameIndex];

        if (data.notEnoughHungerDialogue != null)
        {
            PlayDialogueSequence(
                data.notEnoughHungerDialogue,
                0,
                isPreGame: false,
                onComplete: () => dialogueManager.HideDialogue()
            );
        }
        else
        {
            dialogueManager.ShowLine(
                new DialogueLine { text = "You look hungry. Go eat something first." },
                onAutoAdvance: () => dialogueManager.HideDialogue()
            );
        }
    }

}
