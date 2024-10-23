using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayer : MonoBehaviour
{
    private LevelGenerator levelGenerator;
    [SerializeField]
    private GameObject test;
    
    void Start() {
        levelGenerator = GetComponent<LevelGenerator>();

        Load();
    }

    void Load() {
        levelGenerator.Generate(test);
    }
}
