using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Health bossHealth;
    [SerializeField] private Image CurrentHealth;
    [SerializeField] private Image TotalHealth;

    private void Start() {
        TotalHealth.fillAmount = 1;

        if (gameObject.name == "BeholderBar") 
            bossHealth = Beholder.instance.GetComponent<Health>();
        else 
            bossHealth = Demon.instance.GetComponent<Health>();
        
        BossHealthBarManager.Instance.AssignHealthBar(gameObject);

        gameObject.SetActive(false);
    }
    private void Update()
    {
        CurrentHealth.fillAmount = bossHealth.currentHealth / bossHealth.getMaxHealth();
    }

    public void setHealthBarActive(bool active) {
        gameObject.SetActive(active);
    }

}
