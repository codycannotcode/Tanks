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

        List<string> availableSizes = new List<string>{"1x1", "1x2", "2x1", "2x2"};

        //attempt to fill out tiles, iterate 4 times at most since all tiles will be filled with <= 4 layouts
        for (int i = 0; i < 4 && availableSizes.Count > 0; i++) {
            string sizeChoice = availableSizes[Random.Range(0, availableSizes.Count)];
            Debug.Log("choice: " + sizeChoice);
            switch (sizeChoice) {
                case "1x1":
                Choose1x1(tiles, availableSizes, level);
                    break;
                case "1x2":
                Choose1x2(tiles, availableSizes, level);
                    break;
                case "2x1":
                Choose2x1(tiles, availableSizes, level);
                    break;
                case "2x2":
                Choose2x2(availableSizes, level);
                    break;
            }
            //[1, 1]
            //[1, 1]
            if (tiles[0,0] == 1 && tiles[0,1] == 1 && tiles[1,0] == 1 && tiles[1,1] == 1) {
                availableSizes.Clear();
            }
            // string p = "\n";
            // p += "[" + tiles[0,0] + ", " + tiles[0, 1] + "][" + tiles[1,0] + ", " + tiles[1, 1] + "]\n";
            // Debug.Log(p);
        }

        navMeshSurface.BuildNavMesh();
    }

    void Choose1x1(int[,] tiles, List<string> availableSizes, GameObject level) {
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

        //update available sizes list
        //this is scuffed
        switch (availableSizes.Count) {
            case 4:
            //if all 4 are available, only one of the tiles will be filled in, making 2x2 not possible
            availableSizes.Remove("2x2");
            break;
            case 3:
            //in diagonal cases, we can only place 1x1 layouts, remove all other sizes
            //[0, 1]  [1, 0]
            //[1, 0]  [0, 1]
            if (tiles == new int[,]{{1,0},{0,1}} || tiles == new int[,]{{0,1},{1,0}}) {
                availableSizes.Remove("2x1");
                availableSizes.Remove("1x2");
            }
            //[1, 1]  [0, 0]
            //[0, 0]  [1, 1]
            else if (tiles == new int[,]{{1,1},{0,0}} || tiles == new int[,]{{0,0},{1,1}}) {
                availableSizes.Remove("2x1");
            }
            //[1, 0]  [0, 1]
            //[1, 0]  [0, 1]
            else{
                availableSizes.Remove("1x2");
            }
            break;
            case 2:
            //either removing 2x1 or 1x2
            availableSizes.RemoveAt(1);
            break;
        }
    }

    void Choose1x2(int[,] tiles, List<string> availableSizes, GameObject level) {
        bool flipX = Random.Range(0, 2) == 0;
        bool flipZ;

        // if the matrix is empty, can put layout anywhere
        if (tiles[0,0] == 0 && tiles[0,1] == 0 && tiles[1,0] == 0 && tiles[1,1] == 0) {
            flipZ = Random.Range(0, 2) == 0;
            availableSizes.Remove("2x2");
        }
        //[0, 0]
        //[X, X]
        else if (tiles[0,0] == 0 && tiles[0, 1] == 0) {
            flipZ = false;
            availableSizes.Remove("1x2");
        }
        //[X, X]
        //[0, 0]
        else {
            flipZ = true;
            availableSizes.Remove("1x2");
        }
        availableSizes.Remove("2x1");

        //fill in the tiles that we're now using
        if (flipZ) {
            tiles[1, 0] = 1;
            tiles[1, 1] = 1;
        }
        else {
            tiles[0, 0] = 1;
            tiles[0, 1] = 1;
        }

        GameObject layoutChoice = layouts.size1x2[Random.Range(0, layouts.size1x2.Count)];
        layoutChoice = Instantiate(layoutChoice);
        layoutChoice.transform.SetParent(level.transform, true);

        FlipLayout(layoutChoice.transform, flipX, flipZ);
    }
    
    void Choose2x1(int[,] tiles, List<string> availableSizes, GameObject level) {
        bool flipX;
        bool flipZ = Random.Range(0, 2) == 0;

        // if i is 0, then the matrix is empty, can put layout anywhere
        if (tiles[0,0] == 0 && tiles[0,1] == 0 && tiles[1,0] == 0 && tiles[1,1] == 0) {
            flipX = Random.Range(0, 2) == 0;
            availableSizes.Remove("2x2");
        }
        //[0, X]
        //[0, X]
        else if (tiles[0, 0] == 0 && tiles[1, 0] == 0) {
            flipX = false;
            availableSizes.Remove("2x1");
        }
        else {
            flipX = true;
            availableSizes.Remove("2x1");
        }
        availableSizes.Remove("1x2");

        //fill in the tiles that we're now using
        if (flipX) {
            tiles[0, 1] = 1;
            tiles[1, 1] = 1;
        }
        else {
            tiles[0, 0] = 1;
            tiles[1, 0] = 1;
        }

        GameObject layoutChoice = layouts.size2x1[Random.Range(0, layouts.size2x1.Count)];
        layoutChoice = Instantiate(layoutChoice);
        layoutChoice.transform.SetParent(level.transform, true);

        FlipLayout(layoutChoice.transform, flipX, flipZ);
    }
    
    void Choose2x2(List<string> availableSizes, GameObject level) {
        GameObject layoutChoice = layouts.size2x2[Random.Range(0, layouts.size2x2.Count)];
        layoutChoice = Instantiate(layoutChoice);
        layoutChoice.transform.SetParent(level.transform, true);

        FlipLayout(layoutChoice.transform, Random.Range(0, 2) == 0, Random.Range(0, 2) == 0);
        availableSizes.Clear();
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