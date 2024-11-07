using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Beholder : Enemy
{
    [SerializeField] private AudioClip attackSound;
    public static Beholder instance { get; private set; }
    private BossMovement movement;
    private Vector3 initPos;
    private BeholderSpell spell;
    [SerializeField] private GameObject ArenaCollider;
    [SerializeField] private GameObject Container;
    public bool dead;
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
        getSpell();
        damage = 5;
        cooldown = 1.5f;
        movement = GetComponentInParent<BossMovement>();
        movement.enabled = false;
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Attack();
        cooldownTimer += Time.deltaTime;

        if (gameObject.activeSelf && !dead) {
            movement.enabled = true;
        }
        
        if (Player.instance.GetComponent<Health>().currentHealth <= 0 && !dead) {
            Reset();
        }
    }

    public void Reset() {
        GetComponent<Health>().setHealth(GetComponent<Health>().getMaxHealth());
        BossHealthBarManager.Instance.BeholderActive(false);
        movement.enabled = false;
        gameObject.SetActive(false);
        movement.transform.position = initPos;
        dead = false;
    }


    private void Attack() {
        if (PlayerInSight() && cooldownTimer > cooldown) {
            AudioManager.instance.playSound(attackSound);
            anim.SetTrigger("attack");
            cooldownTimer = 0;
            spell.Activate();
        }

        if (movement != null) {
            movement.enabled = !PlayerInSight();
        }
    }

    private void getSpell() {
        foreach (Transform child in transform) {
            if (child.name == "BeholderSpell") {
                spell = child.GetComponent<BeholderSpell>();
            }
        }
    }

    private void onDeath() {
        dead = true;
        movement.enabled = false;
        StartCoroutine(Message());
    }

    private IEnumerator Message() {
        yield return new WaitForSeconds(1);
        UIManager.instance.PopMessage(1);
    }

}
