using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance {get; private set;}

    [Header("Main Screen")]
    [SerializeField] private GameObject mainScreen;
    
    [Header("Sound")]
    [SerializeField] private AudioClip selectionSound;

    [Header("New Game")]
    [SerializeField] private GameObject newGameScreen;

    [Header("Load Game")]
    [SerializeField] private GameObject noSaveFileMessage;

    [Header("Settings")]
    [SerializeField] private GameObject settingsScreen;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        mainScreen.SetActive(true);
        newGameScreen.SetActive(false);
        settingsScreen.SetActive(false);
        noSaveFileMessage.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (settingsScreen.activeInHierarchy) {
                settingsScreen.SetActive(false);
                mainScreen.SetActive(true);
            }
            else if (newGameScreen.activeInHierarchy) {
                newGameScreen.SetActive(false);
                mainScreen.SetActive(true);
            }
        }
    }

    public void NewGame() {
        mainScreen.SetActive(false);
        newGameScreen.SetActive(true);
        if (selectionSound != null)
            AudioManager.instance.playSound(selectionSound);
        Time.timeScale = 0;
    }

    public void LoadGame() {
        if (selectionSound != null)
            AudioManager.instance.playSound(selectionSound);
        if (SaveManager.instance.SaveExists())
            SaveManager.instance.LoadGame();
        else {
            StartCoroutine(SaveNotFound());
        }
    }

    private IEnumerator SaveNotFound() {
        noSaveFileMessage.SetActive(true);
        yield return new WaitForSeconds(2);
        noSaveFileMessage.SetActive(false);
    }

    public void Quit() {
        if (selectionSound != null)
            AudioManager.instance.playSound(selectionSound);
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void Settings() {
        mainScreen.SetActive(false);
        settingsScreen.SetActive(true);
        if (selectionSound != null)
            AudioManager.instance.playSound(selectionSound);
    }

    public void difficultySet(int diff) {
        if (selectionSound != null)
            AudioManager.instance.playSound(selectionSound);
        DifficultyManager.instance.SetDifficulty(diff);
        SaveManager.instance.DeleteSave();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (Player.instance != null) {
            Player.instance.enabled = true;
            Player.instance.GetComponent<Health>().onRespawn();
            Player.instance.onLoad();
        }
    }

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
