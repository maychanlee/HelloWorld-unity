using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public Button yesButton;
    public Button noButton;

    [Header("Player")]
    private PlayerMovement playerMovement;
    private float originalSpeed;
    private bool dialogueActive = false;


    [Header("Typing")]
    public float typingSpeed = 0.02f;

    private Coroutine typingCoroutine;
    private UnityAction yesAction;
    private UnityAction noAction;

    private void Awake()
    {
        dialoguePanel.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
            originalSpeed = playerMovement.moveSpeed;
    }
    private void Update()
    {
        if (!dialogueActive)
            return;

        if (yesButton.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Y))
        {
            yesButton.onClick.Invoke();
        }

        if (noButton.gameObject.activeSelf && Input.GetKeyDown(KeyCode.N))
        {
            noButton.onClick.Invoke();
        }
    }


    // =========================
    // PUBLIC ENTRY POINT
    // =========================
    public void ShowLine(
        DialogueLine line,
        UnityAction onYes = null,
        UnityAction onNo = null,
        UnityAction onAutoAdvance = null,
        UnityAction onComplete = null)
    {
        if (dialogueActive)
            return;

        dialoguePanel.SetActive(true);
        PausePlayer();

        npcNameText.text = line.speakerName;
        npcNameText.gameObject.SetActive(!string.IsNullOrEmpty(line.speakerName));

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(
            TypeText(line, onYes, onNo, onAutoAdvance, onComplete)
        );
    }


    // =========================
    // TYPING EFFECT
    // =========================
    private IEnumerator TypeText(
        DialogueLine line,
        UnityAction onYes,
        UnityAction onNo,
        UnityAction onAutoAdvance,
        UnityAction onComplete)
    {
        dialogueText.text = "";

        foreach (char c in line.text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Typing finished
        if (line.hasYesNo)
        {
            ShowChoices(
                onYes: () =>
                {
                    onYes?.Invoke();
                    // Do NOT call onComplete here; sequence may continue in PlayDialogueSequence
                },
                onNo: () =>
                {
                    onNo?.Invoke();
                    // Do NOT call onComplete here
                }
            );
        }
        else
        {
            yield return new WaitForSeconds(line.autoAdvanceDelay);
            onAutoAdvance?.Invoke();  // This will call PlayDialogueSequence for next line
            // DO NOT call onComplete here
        }
    }


    private void ShowChoices(UnityAction onYes, UnityAction onNo)
    {
        if (onYes != null)
        {
            yesButton.gameObject.SetActive(true);
            yesButton.onClick.AddListener(() => onYes());
        }

        if (onNo != null)
        {
            noButton.gameObject.SetActive(true);
            noButton.onClick.AddListener(() => onNo());
        }
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueActive = false;
        ResumePlayer();
    }

    private void PausePlayer()
    {
        if (playerMovement != null)
            playerMovement.moveSpeed = 0f;
    }

    private void ResumePlayer()
    {
        if (playerMovement != null)
            playerMovement.moveSpeed = originalSpeed;
    }

}
