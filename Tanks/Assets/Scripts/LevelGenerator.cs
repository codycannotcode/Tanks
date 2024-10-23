using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    private GameObject layout;

    void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    public void Generate(GameObject layout) {
        this.layout = Instantiate(layout);
        navMeshSurface.BuildNavMesh();
    }
}
