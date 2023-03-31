using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    public Color DamageColor = Color.red;
    public Color HealColor = Color.green;
    public float lifetime = 5;
    
    public void Init(float amount)
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        float xChange = Random.value * 2 - 1;
        transform.position = new Vector3(transform.position.x + xChange, transform.position.y, transform.position.z);
        if (amount > 0)
        {
            text.text = "+" + Mathf.FloorToInt(amount);
            text.color = HealColor;
        } else
        {
            text.text = Mathf.FloorToInt(amount).ToString();
            text.color = DamageColor;
        }
        GetComponent<Animator>().SetTrigger("FloatUp");
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            Destroy(gameObject);
        }
    }
}
