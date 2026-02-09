using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    public GameObject root;
    public TextMeshProUGUI promptText;

    public void Show(string prompt)
    {
        root.SetActive(true);
        promptText.text = $"[SPACE] {prompt}";
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}
