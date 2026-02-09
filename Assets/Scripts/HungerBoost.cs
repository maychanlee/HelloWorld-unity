using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerBoost : MonoBehaviour, IInteractable
{
    [SerializeField] private BoxCollider2D hungerBoostPos;
    public int boostAmount;

    public void Interact()
    {
        return;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the "Player" tag
        if (collision.CompareTag("Player"))
        {
            PlayerHunger playerHunger = collision.GetComponent<PlayerHunger>();
            if (playerHunger != null)
            {
                playerHunger.ChangeHunger(boostAmount);
            }
            else
            {
                Debug.LogWarning("Player object has no PlayerHunger component!");
            }
        }
    }
}
