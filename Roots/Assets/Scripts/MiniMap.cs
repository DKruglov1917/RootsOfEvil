using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public GameObject roomImgPrefab;
    public List<GameObject> roomObjs = new List<GameObject>();
    private GameObject prevRoom;

    public Color visitedColor, activeColor;

    private void Awake()
    {
        RoomsPlacer.onRoomPlaced += AddRoomToMap;
        Room.onRoomVisited += VisitRoom;
    }

    private void OnDestroy()
    {
        RoomsPlacer.onRoomPlaced -= AddRoomToMap;
        Room.onRoomVisited -= VisitRoom;
    }

    private void Start()
    {
        roomObjs.Add(transform.GetChild(0).GetChild(0).gameObject);
    }

    public void AddRoomToMap(int x, int y)
    {
        var imgObj = Instantiate(roomImgPrefab);
        imgObj.transform.SetParent(transform.GetChild(0), false);
        imgObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * 15 - 75, y * 15 - 75);
        roomObjs.Add(imgObj);
    }

    public void VisitRoom(int num)
    {
        if (prevRoom) prevRoom.GetComponent<Image>().color = visitedColor;

        roomObjs[num].GetComponent<Image>().color = activeColor;

        prevRoom = roomObjs[num];
    }
}
