using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class StartMenuController : MonoBehaviour
{
    private const string GameSceneName = "SampleScene";

    public void NewGame()
    {
        int slotId = GetSlotIdFromUI();
        SaveSlotManager.Instance.SetSlot(slotId);

        DeleteSave(slotId);
        SceneManager.LoadScene(GameSceneName);
    }

    public void ContinueGame()
    {
        int slotId = GetSlotIdFromUI();
        SaveSlotManager.Instance.SetSlot(slotId);

        SceneManager.LoadScene(GameSceneName);
    }

    private int GetSlotIdFromUI()
    {
        SlotId slot = GetComponentInParent<SlotId>();

        if (slot == null)
        {
            Debug.LogError("StartMenu button is not inside a SlotId object!");
            return 0;
        }

        return slot.slotId;
    }

    private void DeleteSave(int slotId)
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            $"save_slot_{slotId}.json"
        );

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Deleted save for slot {slotId}");
        }
    }
}
