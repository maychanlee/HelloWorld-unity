using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    [SerializeField] private BackendConfig backendConfig;

    private string activeBaseUrl;

    public string BackendBaseUrl => activeBaseUrl;

    private void Awake()
    {
        activeBaseUrl = backendConfig.remoteBaseUrl.TrimEnd('/');
    }

    public IEnumerator SendJson(
        string url,
        string json,
        string method,
        Action<string> onSuccess,
        Action onFailure
    )
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request =
            new UnityWebRequest(url, method);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = backendConfig.timeoutSeconds;

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError(
                $"API ERROR [{request.responseCode}]\n" +
                $"{request.error}\n" +
                $"{request.downloadHandler.text}"
            );
            onFailure?.Invoke();
        }
    }

    public IEnumerator Get(
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
                Debug.LogError(
                    $"GET ERROR [{request.responseCode}]\n" +
                    $"{request.error}"
                );
                onFailure?.Invoke();
            }
        }
    }
}
