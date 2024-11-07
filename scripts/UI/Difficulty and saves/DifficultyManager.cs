using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance {get; private set;}
    private int difficulty;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        
    }

    public void SetDifficulty(int _difficulty) {
        difficulty = _difficulty;
    }

    public int getDifficulty() {
        return difficulty;
    }
}
