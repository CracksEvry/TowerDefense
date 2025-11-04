using UnityEngine;
using UnityEngine.UI;

public class Basehealth : MonoBehaviour
{
    [SerializeField] public int health = 100;
    [Header("Optioneel UI")]
    public Slider slider;

    private void Start()
    {
        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = health;
            slider.value = health;
        }
    }

    public void RemoveHealth(int amount)
    {
        if (amount <= 0) return;
        health = Mathf.Max(0, health - amount);
        if (slider != null) slider.value = health;

        if (health <= 0)
        {
            // TODO: game over / stop waves
            // Debug.Log("BASE DESTROYED");
        }
    }
}