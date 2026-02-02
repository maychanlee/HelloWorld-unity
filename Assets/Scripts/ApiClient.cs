using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    [SerializeField] private BackendConfig backendConfig;

    private string activeBaseUrl;

    private void Awake()
    {
        activeBaseUrl = backendConfig.remoteBaseUrl;
    }

    public IEnumerator GetExample(
        Action<string> onSuccess,
        Action<string> onError
    )
    {
        yield return StartCoroutine(SendRequest(
            $"{activeBaseUrl}/example",
            onSuccess,
            () => TryLocalFallback(onSuccess, onError)
        ));
    }

    private void TryLocalFallback(
        Action<string> onSuccess,
        Action<string> onError
    )
    {
        if (activeBaseUrl == backendConfig.localBaseUrl)
        {
            onError?.Invoke("Both backends unreachable");
            return;
        }

        Debug.LogWarning("Switching to local backend...");
        activeBaseUrl = backendConfig.localBaseUrl;

        StartCoroutine(SendRequest(
            $"{activeBaseUrl}/example",
            onSuccess,
            () => onError?.Invoke("Both backends unreachable")
        ));
    }

    private IEnumerator SendRequest(
        string url,
        Action<string> onSuccess,
        Action onFailure
    )
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.timeout = backendConfig.timeoutSeconds;

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"{url} failed: {request.error}");
                onFailure?.Invoke();
            }
        }
    }
}
