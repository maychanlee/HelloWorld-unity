using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class JokeCall : MonoBehaviour
{
    private const string JOKE_URL = "https://icanhazdadjoke.com/";

    public IEnumerator GetDadJoke(System.Action<string> onSuccess)
    {
        UnityWebRequest request = UnityWebRequest.Get(JOKE_URL);

        request.SetRequestHeader("Accept", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Dad Joke API Error: " + request.error);
            onSuccess?.Invoke("I lost my funny bone...");
            yield break;
        }

        JokeResponse response =
            JsonUtility.FromJson<JokeResponse>(request.downloadHandler.text);

        onSuccess?.Invoke(response.joke);
    }
}

[System.Serializable]
public class JokeResponse
{
    public string id;
    public string joke;
    public int status;
}
