using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Eye : Enemy
{

    
    protected override void Awake()
    {
        base.Awake();
        damage = 1;
        cooldown = 1;
    }

    void Update()
    {
        Attack();
        cooldownTimer += Time.deltaTime;
    }

    private void Attack() {
        if (PlayerInSight() && cooldownTimer > cooldown) {
            anim.SetTrigger("attack");
            cooldownTimer = 0;
        }

        if (enemyPatrol != null) {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

}
