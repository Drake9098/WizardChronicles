using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Health playerHealth;
    [SerializeField] private Image CurrentHealth;
    [SerializeField] private Image TotalHealth;

    private void Start() {
        playerHealth = Player.instance.GetComponent<Health>();
        TotalHealth.fillAmount = 1;
    }
    private void Update()
    {
        CurrentHealth.fillAmount = playerHealth.currentHealth / playerHealth.getMaxHealth();
    }

}
