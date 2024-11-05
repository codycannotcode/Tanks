using static System.Math;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private LayoutData layouts;
    [SerializeField]
    private TankData tankPrefabs;
    private NavMeshSurface navMeshSurface;

    void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    public Level Generate(int levelNumber) {
        GameObject level = new GameObject("Levels");
        int[,] tiles = new int[2,2];

        List<string> availableSizes = new List<string>{"1x1", "1x2", "2x1", "2x2"};

        //attempt to fill out tiles, iterate 4 times at most since all tiles will be filled with <= 4 layouts
        for (int i = 0; i < 4 && availableSizes.Count > 0; i++) {
            string sizeChoice = availableSizes[Random.Range(0, availableSizes.Count)];
            // Debug.Log("choice: " + sizeChoice);
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

        //choose a random quadrant to place the player in, prioritize placing enemy tanks in other quadrants
        int[] playerQuadrant = new int[]{Random.Range(0, 2) == 0 ? 1 : -1, Random.Range(0, 2) == 0 ? 1 : -1};
        List<Vector3> spawnPositions = new List<Vector3>();
        List<Vector3> playerQuadrantPositions = new List<Vector3>();

        foreach (TankSpawn spawn in level.GetComponentsInChildren<TankSpawn>()) {
            Vector3 position = spawn.transform.position;
            if (Sign(position.x) != Sign(playerQuadrant[0]) || Sign(position.z) != Sign(playerQuadrant[1])) {
                spawnPositions.Insert(Random.Range(0, spawnPositions.Count + 1), spawn.transform.position);
            } else {
                playerQuadrantPositions.Insert(Random.Range(0, playerQuadrantPositions.Count + 1), spawn.transform.position);
            }
            Destroy(spawn.gameObject);
        }

        GameObject enemiesFolder = new GameObject("Enemies");
        // see notes for specifics
        // each tier of tank is assigned a point worth
        // based on the level, there is a different probability of each tier spawning
        // based on the level, there is a point total

        int points;
        if (levelNumber < 5) {
            points = levelNumber;
        }
        else if (5 <= levelNumber && levelNumber <= 10) {
            points = 4;
        }
        else {
            points = (int)(levelNumber / 2.5f);
        }

        // {weak, regular, strong}
        int[] weights = new int[] {2, 0, 0};
        weights[1] = System.Math.Min(6, levelNumber / 5);
        weights[2] = System.Math.Max(0, levelNumber - 15) / 5;

        for (int i = 0; i < 12 && points > 0; i++) {
            if (points < 4) {
                weights[2] = 0;
            }
            else if (points < 2) {
                weights[1] = 0;
            }
            // string w = System.String.Format("{0}, {1}, {2}", weights[0], weights[1], weights[2]);
            // Debug.Log(w);

            int tankType = 0;
            int random = Random.Range(1, weights[0] + weights[1] + weights[2] + 1);
            if (random <= weights[0]) {
                tankType = 0; //weak
                points -= 1;
            }
            else if (random > weights[0] && random <= weights[0] + weights[1]) {
                tankType = 1; //regular
                points -= 2;
            }
            else {
                tankType = 2; //strong
                points -= 4;
            }

            GameObject tankChoice = tankPrefabs.weakTanks[Random.Range(0, tankPrefabs.weakTanks.Count)];
            switch (tankType) {
                case 1:
                tankChoice = tankPrefabs.regularTanks[Random.Range(0, tankPrefabs.regularTanks.Count)];
                    break;
                case 2:
                tankChoice = tankPrefabs.strongTanks[Random.Range(0, tankPrefabs.strongTanks.Count)];
                    break;
            }
            
            Vector3 position = spawnPositions[spawnPositions.Count - 1];
            spawnPositions.RemoveAt(spawnPositions.Count - 1);
            position.y = tankChoice.transform.position.y;

            tankChoice = Instantiate(tankChoice, enemiesFolder.transform);
            tankChoice.transform.position = position;
        }

        GameObject playerTank = Instantiate(tankPrefabs.playerTank);
        Vector3 playerPosition = playerQuadrantPositions[0];
        playerPosition.y = playerTank.transform.position.y;
        playerTank.transform.position = playerPosition;

        return new Level(level, playerTank, enemiesFolder);
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

    public Level Generate(Dictionary<GameObject, Vector3> enemyPositions, Vector3 playerPosition) {
        GameObject level = GameObject.Find("Levels");
        
        navMeshSurface.BuildNavMesh();

        GameObject enemiesFolder = GameObject.Find("Enemies");
        foreach (Transform enemyTransform in enemiesFolder.transform) {
            Vector3 position = enemyPositions[enemyTransform.gameObject];
            enemyTransform.SetPositionAndRotation(position, new Quaternion());
        }

        GameObject playerTank = Instantiate(tankPrefabs.playerTank);
        playerTank.transform.position = playerPosition;

        return new Level(level, playerTank, enemiesFolder);
    }
}

public class Level
{
    private GameObject layout;
    public GameObject PlayerTank;
    public GameObject EnemiesFolder;
    public Vector3 PlayerOriginalPosition;
    public Dictionary<GameObject, Vector3> OriginalPositions;
    public GameObject Layout { get {return layout;} }
    public bool Complete { get {return PlayerTank == null || EnemiesFolder.transform.childCount <= 0;} }
    public bool PlayerIsAlive { get {return PlayerTank != null;} }

    public Level(GameObject layout, GameObject playerTank, GameObject enemiesFolder) {
        this.layout = layout;
        this.PlayerTank = playerTank;
        this.EnemiesFolder = enemiesFolder;
        OriginalPositions = new Dictionary<GameObject, Vector3>();

        PlayerOriginalPosition = playerTank.transform.position;
        foreach (Transform enemyTransform in enemiesFolder.transform) {
            OriginalPositions[enemyTransform.gameObject] = enemyTransform.position;
        }

        SetActive(false);
    }

    public void SetActive(bool active) {
        if (PlayerTank != null) {
            PlayerTank.GetComponent<Tank>().enabled = active;
            PlayerTank.GetComponent<PlayerController>().enabled = active;
        }

        foreach (MonoBehaviour script in EnemiesFolder.GetComponentsInChildren<MonoBehaviour>()) {
            script.enabled = active;
        }
        foreach (NavMeshAgent agent in EnemiesFolder.GetComponentsInChildren<NavMeshAgent>()) {
            agent.enabled = active;
        }
    }

    public void Destroy() {
        Object.Destroy(layout);
        Object.Destroy(PlayerTank);
        Object.Destroy(EnemiesFolder);
    }

    // public Dictionary<Vector3, GameObject> GetRemainingTanks() {
    //     Dictionary<Vector3, GameObject> remainingTanks = new Dictionary<Vector3, GameObject>();

    //     foreach (Transform enemyTransform in enemiesFolder.transform) {
    //         if (OriginalPositions.ContainsKey(enemyTransform.gameObject)) {
    //             Debug.Log(enemyTransform.gameObject);
    //             Debug.Log(PrefabUtility.GetCorrespondingObjectFromSource(enemyTransform.gameObject));
    //             remainingTanks[OriginalPositions[enemyTransform.gameObject]] = PrefabUtility.GetCorrespondingObjectFromSource(enemyTransform.gameObject);
    //         }
    //     }
    //     return remainingTanks;
    // }
}