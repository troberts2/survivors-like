using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private const string _level1Name = "TheOldWorld";

    public void TheOldWorldLevel()
    {
        SceneManager.LoadScene(_level1Name);
    }

    public void Level2()
    {
        //TODO
    }

    public void Level3()
    {
        //TODO
    }
}
