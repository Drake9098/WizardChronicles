using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BossMovement : MonoBehaviour
{   
    private Transform target;
    private float speed;
    private Transform boss;
    private Vector3 initScale;
    
    private void Start() {
        DontDestroyOnLoad(gameObject);
        boss = transform.GetChild(0);
        initScale = boss.localScale;
        transform.position = new Vector3(boss.position.x, boss.position.y, boss.position.z);
        target = Player.instance.transform;
        speed = 2.5f;
    }

    private void Update() {
        if (boss.name == "Beholder")
            BeholderChasePlayer();
        else DemonChasePlayer();
    }

    private void BeholderChasePlayer() {
        int direction;
        if (target.position.x <= transform.position.x) {
            direction = -1;
        }
        else {
            direction = 1;
        }
        boss.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        transform.position = new Vector3(transform.position.x + direction * speed * Time.deltaTime, transform.position.y, transform.position.z);
    }

    private void DemonChasePlayer() {
        int direction;
        int currentDirection = boss.localScale.x > 0 ? 1 : -1;
        if (target.position.x <= transform.position.x) {
            direction = 1;
        }
        else {
            direction = -1;
        }
        
        if (currentDirection != direction) {
            enabled = false;
            boss.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
            transform.position = new Vector3(transform.position.x - Mathf.Sign(direction) * 2.5f, transform.position.y, transform.position.z);
            enabled = true;
        }
        transform.position = new Vector3(transform.position.x - direction * speed * Time.deltaTime, transform.position.y, transform.position.z);
    }

}
