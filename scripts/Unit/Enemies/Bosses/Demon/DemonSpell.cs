using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpell : Enemy
{
    [SerializeField] private AudioClip spellSound;

    private void Start() {
        damage = 3;
        Deactivate();
    }

    public void Activate(){
        transform.position = new Vector3(Player.instance.transform.position.x, transform.position.y, transform.position.z);
        gameObject.SetActive(true);
        if (spellSound != null) AudioManager.instance.playSound(spellSound);
    }

    private void Deactivate() {
        gameObject.SetActive(false);
        Demon.instance.isCasting = false;
    }

    public bool PlayerSpellInSight() {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.localScale.y * -range * colliderDistance * transform.up,
         new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y * range, boxCollider.bounds.size.z),
         0, Vector2.down, 0, playerLayer);

        return hit.collider != null;
    }

    public new void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.localScale.x * -range * colliderDistance * transform.up,
        new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y * range, boxCollider.bounds.size.z));
    }

    public void SpellDamagePlayer() {
        if (PlayerSpellInSight())
            Player.instance.GetComponent<Health>().TakeDamage(damage + 2.5f * DifficultyManager.instance.getDifficulty());
    }


}
