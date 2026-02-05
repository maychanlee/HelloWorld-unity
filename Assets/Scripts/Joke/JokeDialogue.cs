using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JokeDialogue : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private List<string> pages = new List<string>();
    private int currentPageIndex;

    private bool isTyping;
    private bool pageFullyShown;

    private void Awake()
    {
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string fullText)
    {
        dialoguePanel.SetActive(true);

        pages = SplitTextIntoPages(fullText);
        currentPageIndex = 0;

        ShowPage();
    }

    private void ShowPage()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(pages[currentPageIndex]));
    }

    private IEnumerator TypeText(string pageText)
    {
        isTyping = true;
        pageFullyShown = false;

        dialogueText.text = "";
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = pageText;

        int totalChars = dialogueText.textInfo.characterCount;

        for (int i = 0; i <= totalChars; i++)
        {
            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        pageFullyShown = true;
        yield return new WaitForSeconds(.5f);
        AdvanceDialogue();
    }

    public void AdvanceDialogue()
    {
        // Skip typing
        if (isTyping)
        {
            dialogueText.maxVisibleCharacters = dialogueText.textInfo.characterCount;
            isTyping = false;
            pageFullyShown = true;
            return;
        }

        // Next page
        if (pageFullyShown && currentPageIndex < pages.Count - 1)
        {
            currentPageIndex++;
            ShowPage();
            return;
        }

        // End dialogue
        CloseDialogue();
    }

    public void CloseDialogue()
    {
        if (dialoguePanel == null) return;

        dialoguePanel.SetActive(false);
    }


    // -----------------------------
    // Pagination logic
    // -----------------------------
    private List<string> SplitTextIntoPages(string fullText)
    {
        List<string> result = new List<string>();

        dialogueText.text = fullText;
        dialogueText.ForceMeshUpdate();

        TMP_TextInfo textInfo = dialogueText.textInfo;

        for (int i = 0; i < textInfo.pageCount; i++)
        {
            int pageStart = textInfo.pageInfo[i].firstCharacterIndex;
            int pageEnd = textInfo.pageInfo[i].lastCharacterIndex;

            int length = pageEnd - pageStart + 1;
            result.Add(fullText.Substring(pageStart, length));
        }

        return result;
    }
}
