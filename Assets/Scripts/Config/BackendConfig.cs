using UnityEngine;

[CreateAssetMenu(menuName = "Config/Backend Config")]
public class BackendConfig : ScriptableObject
{
    [Header("API Base URLs")]
    public string remoteBaseUrl;

    [Header("Settings")]
    public int timeoutSeconds = 5;
}
