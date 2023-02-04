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