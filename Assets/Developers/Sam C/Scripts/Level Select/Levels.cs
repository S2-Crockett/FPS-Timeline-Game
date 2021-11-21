using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Levels", menuName ="Levels/Level", order = 1)]
public class Levels : ScriptableObject  
{
    [Header("Text")]
    public string levelNumber;
    public string levelDifficulty;
    public string levelDescription;

    [Header("Declarations")]
    public int level;
    public Scene scene;
}
