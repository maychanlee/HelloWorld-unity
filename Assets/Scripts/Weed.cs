using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weed : MonoBehaviour
{
    private WeedGameController gameManager;
    
    public void SetManager(WeedGameController manager)
    {
        gameManager = manager;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Notify the game manager
            if (gameManager != null)
            {
                gameManager.OnWeedDestroyed();
            }
                    
            Destroy(gameObject);
        }
    }
}