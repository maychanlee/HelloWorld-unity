using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    public int currentHunger;
    public int maxHunger = 100;

    public void ChangeHunger(int amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        Debug.Log("Current hunger: " + currentHunger);
    }

    void Start()
    {
        currentHunger = 50; // start partially fed
    }
}