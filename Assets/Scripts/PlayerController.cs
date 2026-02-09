using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // prevents duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
