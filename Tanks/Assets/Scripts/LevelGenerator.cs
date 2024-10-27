using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private LayoutData layouts;
    private NavMeshSurface navMeshSurface;
    private GameObject level;

    void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    public void Generate() {
        GameObject level = new GameObject("Levels");
        int[,] tiles = new int[2,2];

        List<string> availableSizes = new List<string>{"1x1"/*, "1x2", "2x1", "2x2"*/};

        //attempt to fill out tiles, iterate 4 times at most since all tiles will be filled with <= 4 layouts
        for (int i = 0; i < 4; i++) {
            string sizeChoice = availableSizes[Random.Range(0, availableSizes.Count)];
            switch (sizeChoice) {
                case "1x1":
                List<int[]> availableTiles = new List<int[]>();
                for (int r = 0; r < 2; r++) {
                    for (int c = 0; c < 2; c++) {
                        if (tiles[r, c] == 0) {
                            availableTiles.Add(new int[]{r, c});
                        }
                    }
                }
                int[] tileChoice = availableTiles[Random.Range(0, availableTiles.Count)];
                tiles[tileChoice[0], tileChoice[1]] = 1;
                //mark a random available tile as taken

                GameObject layoutChoice = layouts.size1x1[Random.Range(0, layouts.size1x1.Count)];
                layoutChoice = Instantiate(layoutChoice);
                layoutChoice.transform.SetParent(level.transform, true);
                if (tileChoice[0] == 1 || tileChoice[1] == 1) {
                    FlipLayout(layoutChoice.transform, tileChoice[1] == 1, tileChoice[0] == 1);
                }
                break;
            }
        }



        navMeshSurface.BuildNavMesh();
    }

    static void FlipLayout(Transform transform, bool xAxis, bool zAxis) {
        Vector3 flipVector = new Vector3(xAxis ? -1 : 1, 1, zAxis ? -1 : 1);
        foreach (Transform child in transform) {
            child.transform.position = Vector3.Scale(child.transform.position, flipVector);
        }
    }
}

public class Level
{
    private GameObject level;
    private GameObject playerTank;
    private GameObject enemiesFolder;

    public Level(GameObject level, GameObject playerTank, GameObject enemiesFolder) {
        this.level = level;
        this.playerTank = playerTank;
        this.enemiesFolder = enemiesFolder;

        SetActive(false);
    }

    public void SetActive(bool active) {
        playerTank.GetComponent<Tank>().enabled = active;
        playerTank.GetComponent<PlayerController>().enabled = active;

        foreach (Tank tank in enemiesFolder.GetComponentsInChildren<Tank>(true)) {
            tank.enabled = active;
        }
        foreach (NavMeshAgent agent in enemiesFolder.GetComponentsInChildren<NavMeshAgent>(true)) {
            agent.enabled = active;
        }
    }
}