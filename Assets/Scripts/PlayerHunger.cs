using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHunger : MonoBehaviour
{
    public int currentHunger;
    public int maxHunger = 100;
    public Slider hungerSlider;

    public void ChangeHunger(int amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        Debug.Log("Current hunger: " + currentHunger);
        hungerSlider.maxValue = maxHunger;
        hungerSlider.value = currentHunger;
    }

    void Start()
    {
        currentHunger = 50; // start partially fed
        hungerSlider.maxValue = maxHunger;
        hungerSlider.value = currentHunger;
    }
}