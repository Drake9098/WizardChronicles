using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Unit
{
    public static Player instance { get; private set; }
    private float horizontalInput;
    private bool grounded;
    private float airTimer;
    private float jumpCooldown;
    private float jumpTimer;
    private float dodgeCooldown;
    private float dodgeTimer;
    private float elapsedTime;
    private Health health;
    public Vector3 spawnPoint;


    [Header("Projectiles")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] shards;

    [Header("Sounds")]
    [SerializeField] private AudioClip iceSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip healSound;


    protected override void Awake()
    {
        base.Awake();
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        onLoad();
        
        health = GetComponent<Health>();
        speed = 2.5f;
        cooldown = 1.2f;
        airTimer = 0;
        jumpCooldown = 0.2f;
        dodgeCooldown = 2;
        jumpTimer = dodgeTimer = Mathf.Infinity;
        elapsedTime = 0;
    }

    private void Update(){
        Move();

        if (Input.GetMouseButton(0) && cooldownTimer > cooldown && CanAttack()) {
            Attack();
        }

        if (Input.GetMouseButton(1)) {
            StartCoroutine(dodge());
        }

        if (getVelocity() < -0.1f && !CheckGrounded()) {
            airTimer += Time.deltaTime;
        }
        
        if (airTimer > 3) {
            health.TakeDamage(health.currentHealth);
            airTimer = 0;
        }

        cooldownTimer += Time.deltaTime;
        if (grounded) jumpTimer += Time.deltaTime;
        dodgeTimer += Time.deltaTime;
    }

    private float getVelocity() {
        return rb.velocity.y;
    }

    private bool CheckGrounded() {
        Vector2 position = new Vector2(transform.position.x, transform.position.y - 0.7f);
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, 0.5f);
        Debug.DrawRay(position, Vector2.down * 0.5f, Color.red);
        if (hit.collider != null && hit.collider.CompareTag("ground"))
            return true;
        else return false;
    }

    public void SpeedUp(bool sign) {
        if (sign) {
            speed += 0.7f;
        }
        else {
            speed -= 0.7f;
        }
    }
    //movement
    private void Move() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(0.7f, 0.7f, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-0.7f,0.7f,1);

        if (Input.GetKey(KeyCode.Space) && grounded)
            jump();

        anim.SetBool("run", horizontalInput != 0 && CheckGrounded());
        anim.SetBool("grounded", grounded);
    }

    private void jump() {
        if (jumpTimer > jumpCooldown) {
            jumpTimer = 0;
            rb.velocity =  new Vector2(rb.velocity.x, speed+1.5f);
            AudioManager.instance.playSound(jumpSound);
            anim.SetTrigger("jump");
            grounded = false;
        }
    }

    private IEnumerator dodge() {
        if (dodgeTimer > dodgeCooldown) {
            health.setInvincibility(true);
            Physics2D.IgnoreLayerCollision(6,7,true);
            rb.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * speed,0);
            rb.gravityScale = 0;
            instance.enabled = false;
            dodgeTimer = 0;
            anim.SetTrigger("dodge");
            
            while (elapsedTime < 0.5f) {
                transform.position += new Vector3(Mathf.Sign(transform.localScale.x) * 0.02f, 0, 0);
                elapsedTime += Time.deltaTime;
                if (health.currentHealth <= 0) {
                    yield break;
                }
                yield return null; 
            }

            elapsedTime = 0;
            rb.gravityScale = 1;
            instance.enabled = true;
            health.setInvincibility(false);
            Physics2D.IgnoreLayerCollision(6,7,false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.gameObject.CompareTag("ground")) {
            grounded = true;
            airTimer = 0;
        }
        
        if (collision.gameObject.CompareTag("Potion")) {
            health.Heal(5);
            if (healSound != null)
                AudioManager.instance.playSound(healSound);
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("BeholderArena") && !Beholder.instance.dead) {
            BossHealthBarManager.Instance.BeholderActive(true);
            Beholder.instance.gameObject.SetActive(true);
        }

        if (other.gameObject.CompareTag("DemonArena") && !Demon.instance.dead) {
            BossHealthBarManager.Instance.DemonActive(true);
            Demon.instance.gameObject.SetActive(true);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.name == "PortalMessageCollider") {
            UIManager.instance.PopMessage(2);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Portal")) {
            transform.position = GameObject.FindWithTag("DemonArena").transform.position;
        }
    }

    //attack
    public bool CanAttack() {
        return horizontalInput == 0 && grounded && cooldownTimer > cooldown && CheckGrounded();
    }

    private void Attack(){
        AudioManager.instance.playSound(iceSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        shards[FindShard()].transform.position = firePoint.position;
        shards[FindShard()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    //object pooling
    private int FindShard(){
        for (int i = 0; i < shards.Length; i++) {
            if (!shards[i].activeInHierarchy) 
                return i;
        }
        return 0;
    }

    public void onLoad() {
        transform.position = new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z);
        enabled = false;
    }
}
