using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : Unit
{
    [SerializeField] protected BoxCollider2D boxCollider;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected float range;
    [SerializeField] protected float colliderDistance;


    private Health playerHealth;
    protected EnemyPatrol enemyPatrol;

    protected override void Awake(){
        base.Awake();
        if (unitName != "Beholder" || unitName != "Demon") enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Start() {
        if (Player.instance == null) return;
        playerHealth = Player.instance.GetComponent<Health>();
    }
    public bool PlayerInSight(){
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.localScale.x * range * colliderDistance * transform.right,
         new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
         0, Vector2.left, 0, playerLayer);
        
        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    public void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.localScale.x * range * colliderDistance * transform.right,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void DamagePlayer(){
        if (PlayerInSight())
            playerHealth.TakeDamage(damage + 2.5f * DifficultyManager.instance.getDifficulty());
    }

    protected void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.CompareTag("Player"))
            playerHealth.TakeDamage(1);
    }

}
