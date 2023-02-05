using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Root.hasHealedRoot)
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene("ChooseNextRoot");
        }
    }
}
