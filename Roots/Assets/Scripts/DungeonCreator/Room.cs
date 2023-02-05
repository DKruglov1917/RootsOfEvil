using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Room : MonoBehaviour
{
    public delegate void roomVisitAction(int num);
    public static event roomVisitAction onRoomVisited;

    public GameObject DoorU;
    public GameObject DoorR;
    public GameObject DoorD;
    public GameObject DoorL;

    public NavMeshSurface navMeshSurface;

    public int num;
    private bool isVisited;

    public GameObject ObstaclesObj;

    private List<GameObject> deactivatedObstacles = new List<GameObject>();

    public GameObject Root, Portal;

    public GameObject Enemies;


    private void Awake()
    {
        if(!ObstaclesObj) return;

        int rnd = Random.Range(0, ObstaclesObj.transform.childCount);

        for (int i = 0; i < rnd; i++)
        {
            int boolRnd = Random.Range(0, 2);

            if (boolRnd == 1)
            {
                while (true)
                {
                    int obstRnd = Random.Range(0, ObstaclesObj.transform.childCount);
                    var curObj = ObstaclesObj.transform.GetChild(obstRnd).gameObject;

                    if (deactivatedObstacles.Contains(curObj)) continue;

                    else
                    {
                        curObj.gameObject.SetActive(false);
                        deactivatedObstacles.Add(curObj);
                        break;
                    }
                }
            }
        }
    }

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        int rnd = Random.Range(0, Enemies.transform.childCount);

        for (int i = 0; i < rnd; i++)
        {
            Enemies.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void RotateRandomly()
    {
        int count = Random.Range(0, 4);

        for (int i = 0; i < count; i++)
        {
            transform.Rotate(0, 90, 0);

            GameObject tmp = DoorL;
            DoorL = DoorD;
            DoorD = DoorR;
            DoorR = DoorU;
            DoorU = tmp;
        }
    }

    private void OnTriggerEnter(Collider col)
    {     
        if(col.tag == "Player")
        {
            isVisited = true;
            onRoomVisited?.Invoke(num);
        }
    }
}