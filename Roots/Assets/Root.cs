using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Root : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;
    public static bool hasHealedRoot;

    private void Start()
    {
        audioSource.clip = clip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            hasHealedRoot = true;
            gameObject.SetActive(false);
            audioSource.Play();
        }
    }
}
