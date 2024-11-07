using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Skeleton : Enemy
{

    [SerializeField] private AudioClip swordSound;

    protected override void Awake()
    {
        base.Awake();
        damage = 3;
        cooldown = 1.5f;
    }

    void Update()
    {
        Attack();
        cooldownTimer += Time.deltaTime;
    }

    private void Attack() {
        if (PlayerInSight() && cooldownTimer > cooldown) {
            AudioManager.instance.playSound(swordSound);
            anim.SetTrigger("attack");
            cooldownTimer = 0;
        }

        if (enemyPatrol != null) {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }
}
