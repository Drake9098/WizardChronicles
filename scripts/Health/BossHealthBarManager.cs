using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBarManager : MonoBehaviour
{
    public static BossHealthBarManager Instance { get; private set; }
    public GameObject BeholderHealthBar;
    public GameObject DemonHealthBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AssignHealthBar(GameObject healthBar) {
        if (healthBar.name == "BeholderBar") {
            BeholderHealthBar = healthBar;
        }
        else DemonHealthBar = healthBar;
    }

    public void BeholderActive(bool active) {
        BeholderHealthBar.SetActive(active);   
    }

    public void DemonActive(bool active) {
        DemonHealthBar.SetActive(active);
    }
}
