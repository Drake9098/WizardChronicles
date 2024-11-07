using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    private AudioClip selectionSound;

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialScreen;
    private bool tutorial;

    [Header("Messages")]
    [SerializeField] private GameObject[] messageScreens;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        tutorial = !SaveManager.instance.SaveExists();
        if (tutorial) tutorialScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !tutorialScreen.activeInHierarchy) {
            if (pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
        }
    }

#region tutorial
    public void CloseTutorial() {
        Time.timeScale = 1;
        tutorialScreen.SetActive(false);
        if (selectionSound != null)
            AudioManager.instance.playSound(selectionSound);
        tutorial = false;
        PopMessage(0);   
    }
#endregion

#region Messages
    public void PopMessage(int message) {
        Player.instance.enabled = false;
        messageScreens[message].SetActive(true);
    }
    
    public void CloseMessage(int message) {
        Player.instance.enabled = true;
        messageScreens[message].SetActive(false);
        if (selectionSound != null)
            AudioManager.instance.playSound(selectionSound);
        if (message == 3) {
            Player.instance.enabled = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
#endregion

#region Game Over
    public void GameOver() {
        gameOverScreen.SetActive(true);
    }

    public void Restart() {
        if (selectionSound != null)
            AudioManager.instance.playSound(selectionSound);
        Player.instance.GetComponent<Health>().onRespawn();
        Player.instance.transform.position = Player.instance.GetComponent<PlayerRespawn>().getLastCheckpoint();
        gameOverScreen.SetActive(false);
    }

    public void Menu() {
        if (selectionSound != null)
            AudioManager.instance.playSound(selectionSound);
        Player.instance.enabled = false;
        if (!Beholder.instance.dead) Beholder.instance.Reset();
        if (!Demon.instance.dead) Demon.instance.Reset();
        SaveManager.instance.SaveGame();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Quit() {
        if (selectionSound != null)
            AudioManager.instance.playSound(selectionSound);
        SaveManager.instance.SaveGame();
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
#endregion

#region Pause
    public void PauseGame(bool status) {
        if (status) {
            Player.instance.enabled = false;
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
        }
        else {
            Player.instance.enabled = true;
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
        } 
    }
#endregion
    
    public void volumeChange() {
        AudioManager.instance.changeVolume(0.1f);
        if (selectionSound != null)
            AudioManager.instance.playSound(selectionSound);
    }

    public void musicChange() {
        AudioManager.instance.musicVolume(0.1f);
        if (selectionSound != null)
            AudioManager.instance.playSound(selectionSound);
    }

}

