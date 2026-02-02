using UnityEngine;
public class MinigameProgressUI : MonoBehaviour
{
    public UnityEngine.UI.Image progressFill;
    public TMPro.TextMeshProUGUI progressText;

    public void UpdateProgress()
    {
        int completed = GameProgressManager.Instance.GetCompletedCount();
        int total = GameProgressManager.Instance.GetTotalMinigameCount();

        progressFill.fillAmount = (float)completed / total;
        progressText.text = $"{completed}/{total}";
    }
}
