using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;

    public int playerId;
    public int slotId;
    public SaveData currentSave;

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
}
