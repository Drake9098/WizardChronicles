using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;

    public void checkRespawn() {
        UIManager.instance.GameOver();     
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Checkpoint")) {
            currentCheckpoint = other.transform;
            if (checkpointSound != null)
                AudioManager.instance.playSound(checkpointSound);
            other.GetComponent<Collider2D>().enabled = false;
            SaveManager.instance.SaveGame();
        }
    }

    public Vector3 getLastCheckpoint() {
        if (currentCheckpoint != null)
            return new Vector3(currentCheckpoint.position.x, currentCheckpoint.position.y, currentCheckpoint.position.z);
        else return Player.instance.spawnPoint;
    }
}
