using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] int level;
    [SerializeField] int nextLevel;
    [SerializeField] bool lastLevel;
    void Start()
    {
        nextLevel = level + 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadNextLevel()
    {
        //if (!lastLevel)
        //{
        string sceneName = "Level " + nextLevel;
        LoadLevel(sceneName);
        //}
    }

    public void ReloadCurrentLevel()
    {
        LoadLevel("Level " + level);
    }
    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}
