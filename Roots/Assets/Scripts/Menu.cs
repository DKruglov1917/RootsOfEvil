using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class Menu : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public CinemachineTrack idle, start;
    public FreeFlyCamera freeFlyCamera;

    private void Start()
    {
        playableDirector.Pause();
    }

    public void StartButton()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        playableDirector.enabled = true;
        playableDirector.Play();
        StartCoroutine("WaitAndStopTheTree");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    IEnumerator WaitAndStopTheTree()
    {
        yield return new WaitForSeconds(3);
        RotateTree.toRotate = false;
        freeFlyCamera.enabled = true;
        Debug.Log("camera end");
    }
}
