using UnityEngine;

public class DadJokeSign : MonoBehaviour
{
    public JokeCall dadJokeService;
    public JokeDialogue dialogueManager;

    private bool hasTriggered;
    private bool isFetching;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (hasTriggered || isFetching) return;
        
        if (dialogueManager != null)
            dialogueManager.CloseDialogue();

        hasTriggered = true;
        isFetching = true;

        dialogueManager.ShowDialogue("playing fetch with the api... here boy! Woof!");

        StartCoroutine(
            dadJokeService.GetDadJoke(joke =>
            {
                dialogueManager.ShowDialogue(joke);
                isFetching = false;
            })
        );
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (dialogueManager == null) return;

        dialogueManager.CloseDialogue();

        hasTriggered = false;
    }


}
