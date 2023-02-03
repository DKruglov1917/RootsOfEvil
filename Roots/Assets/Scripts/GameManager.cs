using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{  
    private void Awake()
    {
        Node.onNodePressed += LoadLevel;
    }

    private void OnDestroy()
    {
        Node.onNodePressed -= LoadLevel;
    }

    private void LoadLevel(NodeType nodeType, LevelDifficulty levelDifficulty, LevelType levelType)
    {
        SceneManager.LoadScene("LocalMap");
    }
}
