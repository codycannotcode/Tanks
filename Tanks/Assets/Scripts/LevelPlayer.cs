using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private List<GameObject> tankPrefabs;
    
    private LevelGenerator levelGenerator;
    
    private GameObject player;
    private Transform enemiesFolder;
    
    void Start() {
        enemiesFolder = GameObject.Find("Enemies").transform;
        levelGenerator = GetComponent<LevelGenerator>();
        Load();
    }

    void Load() {
        levelGenerator.Generate();
        // player = Instantiate(playerPrefab);
        // for (int i = 0; i < 3; i++) {
        //     GameObject tank = Instantiate(tankPrefabs[0]);
        //     tank.transform.position = tank.transform.position + new Vector3(3 * (i + 1), 0, 0);
        //     tank.transform.parent = enemiesFolder;
        // }
        StartCoroutine(WaitForLevelEnd());
    }

    IEnumerator WaitForLevelEnd() {
        yield return new WaitUntil(() => player == null || enemiesFolder.childCount <= 0);
        Debug.Log("chungus");
    }
}
