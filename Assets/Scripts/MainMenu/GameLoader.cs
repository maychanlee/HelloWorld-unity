using UnityEngine;

public class GameLoader : MonoBehaviour
{
    private void Start()
    {
        SaveController saveController = FindObjectOfType<SaveController>();

        if (saveController == null)
        {
            Debug.LogError("SaveController not found in scene!");
            return;
        }

        saveController.LoadGame();
        
        foreach (var npc in FindObjectsOfType<NPCQuestGiver>())
        {
            npc.RefreshProgressFromSave();
        }

        FindObjectOfType<MinigameProgressUI>()?.UpdateProgress();
    }
}
