using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Enemy
{
    [SerializeField] private AudioClip attackSound;
    public static Demon instance { get; private set; }
    private BossMovement movement;
    private Vector3 initPos;
    private Vector3 initScale;
    private DemonSpell spell;
    private float spellTimer;
    private float spellCooldown;
    private float escapeTimer;
    private GameObject ArenaCollider;
    [SerializeField] private GameObject Container;
    public bool dead;
    public bool isCasting;
    public bool isAttacking;
    protected override void Awake()
    {
        base.Awake();

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        initPos = transform.position;
        initScale = transform.localScale;
        getSpell();
        damage = 4;
        cooldown = 1.5f;
        spellCooldown = 3.5f;
        spellTimer = Mathf.Infinity;
        movement = GetComponentInParent<BossMovement>();
        movement.enabled = false;
    }

    private void Start() {
        ArenaCollider = GameObject.FindWithTag("DemonArena");
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Attack();
        cooldownTimer += Time.deltaTime;
        if (!isCasting) spellTimer += Time.deltaTime;
        if (isCasting) escapeTimer += Time.deltaTime;
        anim.SetBool("walk", !PlayerInSight() && !isCasting);

        if (gameObject.activeSelf && !dead) {
            movement.enabled = true;
        }
        
        if (Player.instance.GetComponent<Health>().currentHealth <= 0 && !dead) {
            Reset();
        }

        resetBooleans();
    }

    public void Reset() {
        GetComponent<Health>().setHealth(GetComponent<Health>().getMaxHealth());
        BossHealthBarManager.Instance.DemonActive(false);
        movement.enabled = false;
        gameObject.SetActive(false);
        movement.transform.position = initPos;
        transform.localScale = initScale;
        ArenaCollider.SetActive(true);
        isAttacking = false;
        isCasting = false;
        dead = false;
    }


    private void Attack() {
        if (PlayerInSight() && cooldownTimer > cooldown && !isCasting) {
            isAttacking = true;
            anim.SetTrigger("attack");
            cooldownTimer = 0;
        }

        if (movement != null) {
            movement.enabled = !PlayerInSight() && !isCasting && !isAttacking;
        }

        if (spellTimer > spellCooldown && !PlayerInSight()) {
            isCasting = true;
            spellTimer = 0;
            anim.SetTrigger("cast");
        }
    }

    private void castSpell() {
        spell.Activate();
        escapeTimer = 0;
    }

    private void resetAttack() {
        isAttacking = false;
    }

    private void resetBooleans() {
        if (escapeTimer > 3) {
            isCasting = false;
            escapeTimer = 0;
        }
        if (cooldownTimer > 1f && cooldownTimer < cooldown && isAttacking) isAttacking = false;
    }

    private void PlayAttackSound() {
        if (attackSound != null)
            AudioManager.instance.playSound(attackSound);
    }

    private void getSpell() {
        foreach (Transform child in transform) {
            if (child.name == "DemonSpell") {
                spell = child.GetComponent<DemonSpell>();
            }
        }
    }

    private void disableMovement() {
        movement.enabled = false;
    }

    private void onDeath() {
        dead = true;
        ArenaCollider.SetActive(false);
        gameObject.SetActive(false);
        UIManager.instance.PopMessage(3);
    }
}
