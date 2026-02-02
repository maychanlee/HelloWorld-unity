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
            if (gameManager != null)
            {
                gameManager.OnWeedDestroyed();
            }
                    
            Destroy(gameObject);
        }
    }
}