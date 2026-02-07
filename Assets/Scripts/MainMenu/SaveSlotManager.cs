using UnityEngine;

public class SaveSlotManager : MonoBehaviour
{
    public static SaveSlotManager Instance { get; private set; }

    public int CurrentSlotId { get; private set; } = -1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSlot(int slotId)
    {
        CurrentSlotId = slotId;
        Debug.Log($"Save slot set to: {slotId}");
    }
}
