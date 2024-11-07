using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }
    private int currentScene;
    private Vector3 lastCheckpoint;


    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    public void SaveGame() {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        lastCheckpoint = Player.instance.GetComponent<PlayerRespawn>().getLastCheckpoint();
    }

    public void LoadGame() {
        SceneManager.LoadScene(currentScene);
        if (lastCheckpoint != null)
            Player.instance.transform.position = lastCheckpoint;
        if (Player.instance.GetComponent<Health>().currentHealth <= 0)
            Player.instance.GetComponent<Health>().onRespawn();
        Time.timeScale = 1;
        Player.instance.enabled = true;
    }

    public void DeleteSave() {
        currentScene = 0;
    }

    public bool SaveExists() {
        return currentScene > 0;
    }
}
