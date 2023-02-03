using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dungeoun : MonoBehaviour
{
    [SerializeField] private List<NavMeshSurface> rooms;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            rooms.Add(transform.GetChild(i).GetComponent<NavMeshSurface>());
        }

        BakeNavMeshSurfs();
    }

    private void BakeNavMeshSurfs()
    {
        foreach (var room in rooms)
        {
            room.BuildNavMesh();
        }
    }
}
