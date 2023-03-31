using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider Slider;
    private float maxHealth;
    private float currentHealth;

    public Vector3 Offset;

    public void Start()
    {
        transform.position += Offset;
    }

    public void SetMaxHealth(float amount)
    {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, amount / 100 * 2);
        maxHealth = amount;
        Slider.maxValue = amount;
        Slider.minValue = 0;
    }

    public void SetHealth(float amount)
    {
        amount = Mathf.Clamp(amount, 0f, maxHealth);
        Slider.value = amount;
    }
}
