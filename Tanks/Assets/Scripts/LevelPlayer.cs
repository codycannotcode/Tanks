using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPlayer : MonoBehaviour
{   
    private LevelGenerator levelGenerator;
    public Level currentLevel;   
    
    
    void Start() {
        levelGenerator = GetComponent<LevelGenerator>();
        StartCoroutine(Load());
    }

    IEnumerator Load() {
        // if (SessionStats.Replay) {
        //     currentLevel = levelGenerator.Generate(SessionStats.OriginalPositions, SessionStats.PlayerPosition);
        // }
        // else {
        //     currentLevel = levelGenerator.Generate(5);
        // }
        currentLevel = levelGenerator.Generate(5);
        yield return new WaitForSeconds(1);
        Debug.Log("start");
        currentLevel.SetActive(true);

        StartCoroutine(WaitForLevelEnd());
    }

    IEnumerator WaitForLevelEnd() {
        yield return new WaitUntil(() => currentLevel.Complete);
        
        if (currentLevel.PlayerIsAlive) {
            currentLevel.SetActive(false);
            yield return new WaitForSeconds(1);
            NextLevel();
        }
        else {
            currentLevel.SetActive(false);
            SessionStats.Lives--;
            yield return new WaitForSeconds(1);
            if (SessionStats.Lives <= 0) {
                GameOver();
            }
            else {
                ReplayLevel();
            }
        }
    }

    void NextLevel() {
        SessionStats.Level++;
        SceneManager.LoadScene("Transition");
    }

    void ReplayLevel() {
        SceneManager.LoadScene("Transition");
    }

    void GameOver() {
        Debug.Log("Game over");
        SessionStats.Reset();
        SceneManager.LoadScene("StartMenu");
    }
}
