using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayer : MonoBehaviour
{   
    private LevelGenerator levelGenerator;
    private Level currentLevel;   
    
    
    void Start() {
        levelGenerator = GetComponent<LevelGenerator>();
        StartCoroutine(Load());
    }

    IEnumerator Load() {
        currentLevel = levelGenerator.Generate(2);
        yield return new WaitForSeconds(1);
        Debug.Log("start");
        currentLevel.SetActive(true);

        StartCoroutine(WaitForLevelEnd());
    }

    IEnumerator WaitForLevelEnd() {
        yield return new WaitUntil(() => currentLevel.Complete);
        Debug.Log("chungus");
    }
}
