using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NodeType
{
    CharHP,
    TreeHP
}

public enum LevelDifficulty
{
    Easy,
    Medium,
    Hard,
    Boss
}

public enum LevelType
{
    Roots,
    Dirt
}

public class Node : MonoBehaviour
{
    public delegate void buttonPressed(NodeType nodeType, LevelDifficulty levelDifficulty, LevelType levelType);
    public static event buttonPressed onNodePressed;   

    public NodeType nodeType;
    public LevelDifficulty levelDifficulty;
    public LevelType levelType;
    
    private void OnMouseDown()
    {
        onNodePressed?.Invoke(nodeType, levelDifficulty, levelType);
    }
}
