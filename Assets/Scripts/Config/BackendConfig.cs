using UnityEngine;

[CreateAssetMenu(menuName = "Config/Backend Config")]
public class BackendConfig : ScriptableObject
{
    [Header("API Base URLs")]
    public string remoteBaseUrl;
    public string localBaseUrl;

    [Header("Settings")]
    public int timeoutSeconds = 5;
}
