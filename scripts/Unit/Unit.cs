using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    public string unitName;
    protected float speed;
    protected float cooldown;

    protected float cooldownTimer;
    protected int damage;

    protected virtual void Awake(){
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cooldownTimer = Mathf.Infinity;
    }

}
