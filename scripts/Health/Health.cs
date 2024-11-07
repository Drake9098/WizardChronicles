using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    public float currentHealth {get; private set;}
    private Animator anim;
    private bool dead;
    private bool isInvincible;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int flashes;
    private SpriteRenderer spriteRenderer;

    [Header("Components")]
    [SerializeField] Behaviour[] components;

    [Header("Sounds")]
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip deathSound;


    private void Awake(){
        if (!gameObject.CompareTag("Player")) {
            maxHealth += DifficultyManager.instance.getDifficulty() * 5;
        }
        currentHealth = maxHealth;
        dead = false;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isInvincible = false;
    }

    public void TakeDamage(float _damage){
        if (isInvincible) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, maxHealth);

        if (currentHealth > 0) {
            AudioManager.instance.playSound(hurtSound);
            anim.SetTrigger("hurt");
            StartCoroutine(Invulnerabilty());
        }
        else {
            if (!dead) {
                anim.SetTrigger("hurt");
                AudioManager.instance.playSound(deathSound);
                anim.SetTrigger("die");
                GetComponent<Rigidbody2D>().gravityScale = 0;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                foreach (Behaviour comp in components)
                    comp.enabled = false;
                
                dead = true;                   
            }
        }
    }

    public void Heal(float _heal){
        currentHealth = Mathf.Clamp(currentHealth + _heal, 0, maxHealth);
    }

    public void setHealth(float health) {
        currentHealth = health;
    }

    public void onRespawn() {
        dead = false;
        currentHealth = maxHealth;
        anim.ResetTrigger("die");
        anim.Play("idle");
        GetComponent<Rigidbody2D>().gravityScale = 1;

        foreach (Behaviour comp in components)
            comp.enabled = true;
    }

    private IEnumerator Invulnerabilty() {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        Player.instance.SpeedUp(true);
        isInvincible = true;
        for (int i = 0; i < flashes; i++) {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (flashes * 2.5f));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (flashes * 2.5f));
        }
        Player.instance.SpeedUp(false);
        isInvincible = false;
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    public float getMaxHealth() {
        return maxHealth;
    }

    public void setInvincibility(bool condition) {
        isInvincible = condition;
    }
}
