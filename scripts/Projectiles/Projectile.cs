using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    static private float speed = 4;
    private float lifetime;
    private bool hit;
    private float direction;
    private BoxCollider2D BoxCollider;
    private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();
        BoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() {

        if (hit) return;
        
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed,0,0);
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Checkpoint") && !collision.CompareTag("Player")) {
            hit = true;
            BoxCollider.enabled = false;
            anim.SetTrigger("hit");
        }
        
        if (collision.CompareTag("Enemy"))
            collision.GetComponent<Health>().TakeDamage(5);
    }

    public void SetDirection(float _direction){
        direction = _direction;
        gameObject.SetActive(true);
        lifetime = 3;
        hit = false;
        BoxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Math.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;
        
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate(){
        gameObject.SetActive(false);
    }
}
