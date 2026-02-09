using UnityEngine;

public class SaveSlotManager : MonoBehaviour
{
    public static SaveSlotManager Instance { get; private set; }

    public int CurrentSlotId { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
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
    }
}
