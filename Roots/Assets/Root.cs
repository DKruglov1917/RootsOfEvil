using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Root : MonoBehaviour
{
    public static bool hasHealedRoot;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            hasHealedRoot = true;
            gameObject.SetActive(false);
        }
    }
}
