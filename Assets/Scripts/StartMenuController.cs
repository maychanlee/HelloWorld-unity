using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void ContinueGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void NewGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
