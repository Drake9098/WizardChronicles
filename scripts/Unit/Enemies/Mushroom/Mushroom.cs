using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Mushroom : Enemy
{

    [SerializeField] private AudioClip punchSound;
    protected override void Awake()
    {
        base.Awake();
        damage = 2;
        cooldown = 1.5f;
    }

    void Update()
    {
        Attack();
        cooldownTimer += Time.deltaTime;
    }

    private void Attack() {
        if (PlayerInSight() && cooldownTimer > cooldown) {
            anim.SetTrigger("attack");
            AudioManager.instance.playSound(punchSound);
            cooldownTimer = 0;
        }

        if (enemyPatrol != null) {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

}
