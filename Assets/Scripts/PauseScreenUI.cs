using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PauseScreenUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text neighborNameText;
    [SerializeField] private TMP_Text completionPhraseText;

    [Header("GameProgress Reference")]
    [SerializeField] private GameProgressManager progressManager;

    // Optional: keep track of which minigame to display
    private MinigameSaveEntry currentEntry;

    public void ShowMinigameCompletion(int neighborId, int minigameId)
    {
        if (progressManager == null)
        {
            Debug.LogError("GameProgressManager not assigned!");
            return;
        }

        currentEntry = progressManager.GetMinigameResult(neighborId, minigameId);

        if (currentEntry != null)
        {
            neighborNameText.text = currentEntry.neighborName;
            completionPhraseText.text = currentEntry.completionPhrase;
        }
        else
        {
            neighborNameText.text = "Unknown";
            completionPhraseText.text = "???";
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
