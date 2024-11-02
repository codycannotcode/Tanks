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
        currentLevel = levelGenerator.Generate(PlayerStats.level);
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
            Debug.Log(String.Format("Level {0} Completed", PlayerStats.level));
            currentLevel.Destroy();
            NextLevel();
        }
        else {
            currentLevel.SetActive(false);
            currentLevel.Destroy();
        }
    }

    void NextLevel() {
        PlayerStats.level++;
        SceneManager.LoadScene("Transition");
    }
}
