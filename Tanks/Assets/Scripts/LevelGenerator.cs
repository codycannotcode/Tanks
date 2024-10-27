using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private LayoutData layouts;
    private NavMeshSurface navMeshSurface;
    

    void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    public void Generate() {
        //this.layout = Instantiate(layout);
        navMeshSurface.BuildNavMesh();
    }
}
